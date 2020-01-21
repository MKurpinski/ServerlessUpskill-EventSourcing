using System.Threading.Tasks;
using Upskill.Search.Enums;
using Upskill.Search.Managers;
using Upskill.Search.Models;

namespace Upskill.Search.Resolvers
{
    public class IndexNameResolver : IIndexNameResolver
    {
        private readonly IIndexManager _indexManager;

        public IndexNameResolver(
            IIndexManager indexManager)
        {
            _indexManager = indexManager;
        }

        public async Task<string> ResolveIndexName<T>(IndexType indexStatus) where T: ISearchable
        {
            var indexNameResult = await _indexManager.GetIndexNameByType<T>(indexStatus);

            if (indexNameResult.Success)
            {
                return indexNameResult.Value;
            }

            await _indexManager.BuildIndex<T>();
            indexNameResult = await _indexManager.GetIndexNameByType<T>(indexStatus);

             return indexNameResult.Value;
        }
    }
}