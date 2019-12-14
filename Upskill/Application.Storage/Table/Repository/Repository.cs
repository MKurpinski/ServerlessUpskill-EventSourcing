using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Storage.Table.Extensions;
using Application.Storage.Table.Providers;
using Microsoft.WindowsAzure.Storage.Table;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Application.Storage.Table.Repository
{
    public abstract class Repository<T> where T: TableEntity, new()
    {
        private readonly string _nameOfTypeT;
        protected readonly ITableClientProvider TableClientProvider;

        protected Repository(ITableClientProvider tableClientProvider)
        {
            TableClientProvider = tableClientProvider;
            _nameOfTypeT = typeof(T).Name;
        }

        public async Task CreateOrUpdate(T entity)
        {
            var table = await TableClientProvider.Get(_nameOfTypeT);
            var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            await table.ExecuteAsync(insertOrMergeOperation);
        }

        public async Task<IDataResult<T>> GetById(string id)
        {
            var table = await TableClientProvider.Get(_nameOfTypeT);
            var retrieveOperation = TableOperation.Retrieve<T>(_nameOfTypeT, id);
            var result = await table.ExecuteAsync(retrieveOperation);
            
            var entity = result.Result as T;

            if (entity == null)
            {
                return new FailedDataResult<T>();
            }

            return new SuccessfulDataResult<T>(entity);
        }

        public async Task<IList<T>> GetBy(TableQuery<T> tableQuery)
        {
            var table = await TableClientProvider.Get(_nameOfTypeT);

            var result = await table.ExecuteQueryAsync(tableQuery);

            return result;
        }
    }
}
