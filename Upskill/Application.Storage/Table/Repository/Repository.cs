using System.Threading.Tasks;
using Application.Storage.Table.Providers;
using Microsoft.WindowsAzure.Storage.Table;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Application.Storage.Table.Repository
{
    public abstract class Repository<T> where T: TableEntity
    {
        private readonly string _nameOfTypeT;
        private readonly ITableClientProvider _tableClientProvider;

        protected Repository(ITableClientProvider tableClientProvider)
        {
            _tableClientProvider = tableClientProvider;
            _nameOfTypeT = typeof(T).Name;
        }

        public async Task CreateOrUpdate(T entity)
        {
            var table = await _tableClientProvider.Get(_nameOfTypeT);
            var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            await table.ExecuteAsync(insertOrMergeOperation);
        }

        public async Task<IDataResult<T>> GetById(string id)
        {
            var table = await _tableClientProvider.Get(_nameOfTypeT);
            var retrieveOperation = TableOperation.Retrieve<T>(_nameOfTypeT, id);
            var result = await table.ExecuteAsync(retrieveOperation);
            
            var entity = result.Result as T;

            if (entity == null)
            {
                return new FailedDataResult<T>();
            }

            return new SuccessfulDataResult<T>(entity);
        }
    }
}
