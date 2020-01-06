using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Events.ApplicationChangedEvent;
using Application.DataStorage.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Publishers;

namespace Application.Core.Handlers
{
    public class CategoryNameChangedHandler : ICategoryNameChangedHandler
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryNameChangedHandler> _logger;

        public CategoryNameChangedHandler(
            IApplicationRepository applicationRepository,
            ILogger<CategoryNameChangedHandler> logger,
            IEventPublisher eventPublisher,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _logger = logger;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
        }

        public async Task HandleCategoryNameChange(string oldCategoryName, string newCategoryName)
        {
            var oldCategoryApplications = await _applicationRepository.GetByCategoryName(oldCategoryName);

            if (!oldCategoryApplications.Success)
            {
                // push the event back and try to consume later
                _logger.LogError($"Failed applications update for category old name: {oldCategoryName} to new categoryName: {newCategoryName}");
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
                _logger.LogError($"Failed applications update for category old name: {oldCategoryName} to new categoryName: {newCategoryName}" +
                                 $"for applications: {string.Join(",", result.FailedUpdatedDocuments.Select(x => x.Id))}");
            }


            await this.PublishEvents(result.SuccessfulUpdatedDocuments);
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