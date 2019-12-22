using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataStorage.Extensions;
using Application.DataStorage.Options;
using Application.DataStorage.Providers;
using Application.DataStorage.Results;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Application.DataStorage.Repositories
{
    public class ApplicationRepository : BaseRepository<Models.Application>, IApplicationRepository
    {
        protected override string ContainerId => "applications";
        protected override string PartitionKeyPath => $"/{nameof(Models.Application.Address).ToLowerInvariant()}/{nameof(Models.Application.Address.Country).ToLower()}";

        public ApplicationRepository(
            IContainerClientProvider containerClientProvider,
            IOptions<DataStorageOptions> optionsAccessor) 
            : base(containerClientProvider, optionsAccessor)
        {
        }

        public async Task<IDataResult<Models.Application>> Create(Models.Application application)
        {
            try
            {
                var container = await this.GetClient();
                var response = await container.CreateItemAsync(application, new PartitionKey(application.Address.Country));

                if (!response.StatusCode.IsSuccessfulStatusCode())
                {
                    return new FailedDataResult<Models.Application>(nameof(Models.Application).ToLower(),
                        response.Diagnostics.ToString());
                }

                return new SuccessfulDataResult<Models.Application>(response.Resource);
            }
            catch (Exception ex)
            {
                return new FailedDataResult<Models.Application>(nameof(Models.Application).ToLower(), $"{ex.Message} \n{ex.StackTrace}");
            }
        }

        public async Task<IDataResult<IReadOnlyCollection<Models.Application>>> GetByCategoryName(string categoryName)
        {
            try
            {
                var documents = await this.GetByField(nameof(Models.Application.Category).ToLower(), categoryName);
                return new SuccessfulDataResult<IReadOnlyCollection<Models.Application>>(documents);
            }
            catch (Exception)
            {
                return new FailedDataResult<IReadOnlyCollection<Models.Application>>();
            }
        }

        public async Task<BulkUpdateResult<Models.Application>> BulkUpdateDocuments(IReadOnlyCollection<Models.Application> documentsToUpdate)
        {
            try
            {
                var container = await this.GetClient();

                var updateTasks = documentsToUpdate.Select(x =>
                    container.ReplaceItemAsync(x, x.Id, new PartitionKey(x.Address.Country)));

                var results = await Task.WhenAll(updateTasks);

                var failedUpdates = results.Where(x => !x.StatusCode.IsSuccessfulStatusCode()).Select(x => x.Resource);
                var successfulUpdates =
                    results.Where(x => x.StatusCode.IsSuccessfulStatusCode()).Select(x => x.Resource);

                return new BulkUpdateResult<Models.Application>(successfulUpdates.ToList(), failedUpdates.ToList());
            }
            catch (Exception)
            {
                return new BulkUpdateResult<Models.Application>(Enumerable.Empty<Models.Application>().ToList(), documentsToUpdate.ToList());
            }
        }
    }
}
