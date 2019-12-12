using System;
using System.Threading.Tasks;
using Application.DataStorage.Extensions;
using Application.DataStorage.Options;
using Application.DataStorage.Providers;
using Microsoft.Extensions.Options;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Application.DataStorage.Repositories
{
    public class ApplicationRepository : BaseRepository, IApplicationRepository
    {
        protected override string ContainerId => "applications";
        protected override string PartitionKey => nameof(Model.Application.Category);

        public ApplicationRepository(
            IContainerClientProvider containerClientProvider,
            IOptions<DataStorageOptions> optionsAccessor) 
            : base(containerClientProvider, optionsAccessor)
        {
        }

        public async Task<IDataResult<Model.Application>> Create(Model.Application application)
        {
            try
            {
                var container = await this.GetClient();
                var response = await container.CreateItemAsync(application);

                if (!response.StatusCode.IsSuccessfulStatusCode())
                {
                    return new FailedDataResult<Model.Application>(nameof(Model.Application).ToLower(),
                        response.Diagnostics.ToString());
                }

                return new SuccessfulDataResult<Model.Application>(response.Resource);
            }
            catch (Exception ex)
            {
                return new FailedDataResult<Model.Application>(nameof(Model.Application).ToLower(), $"{ex.Message} \n{ex.StackTrace}");
            }
        }
    }
}
