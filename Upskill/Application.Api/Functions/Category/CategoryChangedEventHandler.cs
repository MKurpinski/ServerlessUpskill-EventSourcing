using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Api.Events.External.ApplicationChanged;
using Application.Api.Events.External.Category;
using Application.DataStorage.Repositories;
using Application.DataStorage.Results;
using Application.Storage.Dtos;
using Application.Storage.Tables.Repositories;
using AutoMapper;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Upskill.EventPublisher.Publishers;

namespace Application.Api.Functions.Category
{
    public class CategoryChangedEventHandler
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public CategoryChangedEventHandler(
            ICategoryRepository categoryRepository,
            IApplicationRepository applicationRepository,
            IMapper mapper,
            IEventPublisher eventPublisher)
        {
            _categoryRepository = categoryRepository;
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        [FunctionName(nameof(CategoryChangedEventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent,
            ILogger log)
        {
            var categoryChangedEvent = JsonConvert.DeserializeObject<CategoryChangedEvent>(eventGridEvent.Data.ToString());

            var existingCategoryResult = await _categoryRepository.GetById(categoryChangedEvent.Id);

            if (existingCategoryResult.Success && !existingCategoryResult.Value.Name.Equals(categoryChangedEvent.Name))
            {
                await this.UpdateChangedApplications(existingCategoryResult.Value.Name, categoryChangedEvent.Name, log);
            }

            await _categoryRepository.CreateOrUpdate(new CategoryDto(categoryChangedEvent.Id, categoryChangedEvent.Name));

            log.LogInformation($"{nameof(CategoryChangedEventHandler)}: category: {categoryChangedEvent.Id}");
        }

        private async Task UpdateChangedApplications(string oldCategoryName, string newCategoryName, ILogger logger)
        {
            var oldCategoryApplications = await _applicationRepository.GetByCategoryName(oldCategoryName);

            if (!oldCategoryApplications.Success)
            {
                // push the event back and try to consume later
                logger.LogError($"Failed applications update for category old name: {oldCategoryName} to new categoryName: {newCategoryName}");
                return;
            }

            var applicationsToUpdate = oldCategoryApplications.Value.ToList();

            foreach (var applicationToUpdate in applicationsToUpdate)
            {
                applicationToUpdate.ChangeCategory(newCategoryName);
            }

            var result = await _applicationRepository.BulkUpdateDocuments(applicationsToUpdate);


            if (!result.Success)
            {
                //try to retry the update
                logger.LogError($"Failed applications update for category old name: {oldCategoryName} to new categoryName: {newCategoryName}" +
                                $"for applications: {string.Join(',', result.FailedUpdatedDocuments.Select(x => x.Id))}");
                return;
            }

            await PublishEvents(result);
        }

        private async Task PublishEvents(BulkUpdateResult<DataStorage.Models.Application> result)
        {
            var eventsToPush =
                _mapper
                    .Map<IReadOnlyCollection<DataStorage.Models.Application>, IReadOnlyCollection<ApplicationChangedEvent>>(
                        result.SuccessfulUpdatedDocuments);

            foreach (var toPush in eventsToPush)
            {
                await _eventPublisher.PublishEvent(toPush);
            }
        }
    }
}
