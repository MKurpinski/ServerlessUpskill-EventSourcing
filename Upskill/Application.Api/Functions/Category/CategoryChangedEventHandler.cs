using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Api.Events.External.ApplicationChanged;
using Application.Api.Events.External.Category;
using Application.Category.Handlers;
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
        private readonly ICategoryNameChangedHandler _categoryNameChangedHandler;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;

        public CategoryChangedEventHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IEventPublisher eventPublisher,
            ICategoryNameChangedHandler categoryNameChangedHandler)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _categoryNameChangedHandler = categoryNameChangedHandler;
        }

        [FunctionName(nameof(CategoryChangedEventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent,
            ILogger log)
        {
            var categoryChangedEvent = JsonConvert.DeserializeObject<CategoryChangedEvent>(eventGridEvent.Data.ToString());

            var existingCategoryResult = await _categoryRepository.GetById(categoryChangedEvent.Id);

            if (existingCategoryResult.Success && !existingCategoryResult.Value.Name.Equals(categoryChangedEvent.Name))
            {
                await this.HandleCategoryNameChange(existingCategoryResult.Value.Name, categoryChangedEvent.Name);
            }

            await _categoryRepository.CreateOrUpdate(new CategoryDto(categoryChangedEvent.Id, categoryChangedEvent.Name));

            log.LogInformation($"{nameof(CategoryChangedEventHandler)}: category: {categoryChangedEvent.Id}");
        }

        private async Task HandleCategoryNameChange(string oldCategoryName, string newCategoryName)
        {
            var updatedApplications =
               await _categoryNameChangedHandler.HandleCategoryNameChange(oldCategoryName, newCategoryName);
            await PublishEvents(updatedApplications);
        }

        private async Task PublishEvents(IReadOnlyCollection<DataStorage.Models.Application> updatedApplications)
        {
            var eventsToPush =
                _mapper
                    .Map<IReadOnlyCollection<DataStorage.Models.Application>, IReadOnlyCollection<ApplicationChangedEvent>>(
                        updatedApplications);

            foreach (var toPush in eventsToPush)
            {
                await _eventPublisher.PublishEvent(toPush);
            }
        }
    }
}
