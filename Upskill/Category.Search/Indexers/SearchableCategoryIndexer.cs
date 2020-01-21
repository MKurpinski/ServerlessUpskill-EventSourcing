using System.Threading.Tasks;
using Category.Search.Dtos;
using Category.Search.Models;
using Microsoft.Extensions.Logging;
using Upskill.Results;
using Upskill.Search.Enums;
using Upskill.Search.Indexers;
using Upskill.Search.Providers;

namespace Category.Search.Indexers
{
    public class SearchableCategoryIndexer : BaseIndexer<SearchableCategory>, ISearchableCategoryIndexer
    {
        public SearchableCategoryIndexer(
            ILogger<SearchableCategoryIndexer> logger,
            ISearchIndexClientProvider searchIndexClientProvider) : base(searchIndexClientProvider, logger)
        {
        }

        public async Task Index(CategoryDto toIndex)
        {
            await this.IndexInternal(toIndex, IndexType.Active);
        }

        public async Task Reindex(CategoryDto toIndex)
        {
            await this.IndexInternal(toIndex, IndexType.InProgress);
        }

        public async Task<IResult> Delete(string id)
        {
            return await this.DeleteInternal(id);
        }

        private async Task IndexInternal(CategoryDto toIndex, IndexType indexType)
        {
            var searchableCategory = new SearchableCategory(toIndex.Id, toIndex.Name, toIndex.Description, toIndex.SortOrder);
            await this.Index(searchableCategory, indexType);
        }
    }
}