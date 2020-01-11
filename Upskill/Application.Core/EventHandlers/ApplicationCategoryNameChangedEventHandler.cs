using System.Threading.Tasks;
using Application.Core.Events;
using Application.Search.Handlers;
using Application.Search.Indexers;
using Application.Search.Queries;
using Microsoft.Extensions.Logging;
using Upskill.Events;

namespace Application.Core.EventHandlers
{
    public class ApplicationCategoryNameChangedEventHandler : IEventHandler<ApplicationCategoryNameChangedEvent>
    {
        private readonly ISearchableApplicationIndexer _searchableApplicationIndexer;
        private readonly IApplicationSearchHandler _applicationSearchHandler;
        private readonly ILogger<ApplicationCategoryNameChangedEventHandler> _logger;

        public ApplicationCategoryNameChangedEventHandler(
            ISearchableApplicationIndexer searchableApplicationIndexer,
            ILogger<ApplicationCategoryNameChangedEventHandler> logger,
            IApplicationSearchHandler applicationSearchHandler)
        {
            _searchableApplicationIndexer = searchableApplicationIndexer;
            _logger = logger;
            _applicationSearchHandler = applicationSearchHandler;
        }

        public async Task Handle(Events.ApplicationCategoryNameChangedEvent applicationCategoryNameChangedEvent)
        {
            _logger.LogInformation($"{nameof(Events.ApplicationCategoryNameChangedEvent)} with id: {applicationCategoryNameChangedEvent.Id}, processing started");
            var applicationGetResult = await _applicationSearchHandler.GetById(new GetApplicationByIdQuery(applicationCategoryNameChangedEvent.Id));

            if (!applicationGetResult.Success)
            {
                _logger.LogInformation($"{nameof(Events.ApplicationCategoryNameChangedEvent)}: application with id: {applicationCategoryNameChangedEvent.Id} cannot be found");
            }

            var application = applicationGetResult.Value;
            application.Category = applicationCategoryNameChangedEvent.NewCategoryName;
            await _searchableApplicationIndexer.Index(application);

            _logger.LogInformation($"{nameof(Events.ApplicationCategoryNameChangedEvent)} with id: {applicationCategoryNameChangedEvent.Id}, processing finished");
        }
    }
}
