using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Storage.Extensions;
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

        protected async Task<IResult> CreateOrUpdate(T entity)
        {
            var table = await _tableClientProvider.Get(_nameOfTypeT);
            var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            var result = await table.ExecuteAsync(insertOrMergeOperation);
            
            return this.ResultBasedOnTableResult(result);
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

        protected async Task<IList<T>> GetByField(string fieldName, string value)
        {
            var partitionKeyCondition = TableQuery.GenerateFilterCondition(fieldName, QueryComparisons.Equal, value);

            var query = new TableQuery<T>().Where(partitionKeyCondition);

            var table = await _tableClientProvider.Get(_nameOfTypeT);

            var result = await table.ExecuteQueryAsync(query);

            return result;
        }

        protected async Task<IList<T>> GetBy(TableQuery<T> tableQuery)
        {
            var table = await _tableClientProvider.Get(_nameOfTypeT);

            var result = await table.ExecuteQueryAsync(tableQuery);

            return result;
        }

        protected async Task<IResult> DeleteById(string rowKey)
        {
            var result = await this.GetByIdInternal(rowKey);

            if (result == null)
            {
                return new FailedResult();
            }

            var table = await _tableClientProvider.Get(_nameOfTypeT);
            var tableResult = await table.ExecuteAsync(TableOperation.Delete(result));
            return this.ResultBasedOnTableResult(tableResult);
        }

        private async Task<T> GetByIdInternal(string id)
        {
            var table = await _tableClientProvider.Get(_nameOfTypeT);
            var retrieveOperation = TableOperation.Retrieve<T>(_nameOfTypeT, id);
            var result = await table.ExecuteAsync(retrieveOperation);

            var entity = result.Result as T;

            return entity;
        }
        private IResult ResultBasedOnTableResult(TableResult result)
        {
            if (!result.HttpStatusCode.IsSuccessfulStatusCode())
            {
                return new FailedResult();
            }

            return new SuccessfulResult();
        }
    }
}
