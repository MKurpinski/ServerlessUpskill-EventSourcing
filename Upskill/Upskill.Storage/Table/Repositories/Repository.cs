using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Storage.Table.Extensions;
using Upskill.Storage.Table.Providers;

namespace Upskill.Storage.Table.Repositories
{
    public abstract class Repository<T> where T: TableEntity, new()
    {
        private readonly string _nameOfTypeT;
        private readonly ITableClientProvider _tableClientProvider;

        protected Repository(ITableClientProvider tableClientProvider)
        {
            _tableClientProvider = tableClientProvider;
            _nameOfTypeT = typeof(T).Name;
        }

        protected async Task CreateOrUpdate(T entity)
        {
            var table = await _tableClientProvider.Get(_nameOfTypeT);
            var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            await table.ExecuteAsync(insertOrMergeOperation);
        }

        protected async Task<IDataResult<T>> GetById(string id)
        {
            var entity = await this.GetByIdInternal(id);

            if (entity == null)
            {
                return new FailedDataResult<T>();
            }

            return new SuccessfulDataResult<T>(entity);
        }

        protected async Task<IList<T>> GetBy(TableQuery<T> tableQuery)
        {
            var table = await _tableClientProvider.Get(_nameOfTypeT);

            var result = await table.ExecuteQueryAsync(tableQuery);

            return result;
        }

        protected async Task DeleteById(string rowKey)
        {
            var result = await this.GetByIdInternal(rowKey);

            if (result == null)
            {
                return;
            }

            var table = await _tableClientProvider.Get(_nameOfTypeT);
            await table.ExecuteAsync(TableOperation.Delete(result));
        }

        private async Task<T> GetByIdInternal(string id)
        {
            var table = await _tableClientProvider.Get(_nameOfTypeT);
            var retrieveOperation = TableOperation.Retrieve<T>(_nameOfTypeT, id);
            var result = await table.ExecuteAsync(retrieveOperation);

            var entity = result.Result as T;

            return entity;
        }
    }
}
