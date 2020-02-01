using System.Threading.Tasks;
using Category.Search.Dtos;
using Category.Search.Handlers;
using Category.Search.Models;
using Category.Search.Queries;
using Microsoft.Extensions.Logging;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Search.Enums;
using Upskill.Search.Indexers;
using Upskill.Search.Providers;

namespace Category.Search.Indexers
{
    public class SearchableCategoryIndexer : BaseIndexer<SearchableCategory>, ISearchableCategoryIndexer
    {

        public SearchableCategoryIndexer(
            ILogger<SearchableCategoryIndexer> logger,
            ISearchIndexClientProvider searchIndexClientProvider)
            : base(searchIndexClientProvider, logger)
        {
        }

        public async Task<IResult> Index(CategoryDto toIndex)
        {
            return await this.IndexInternal(toIndex, IndexType.Active);
        }

        public async Task<IResult> Reindex(CategoryDto toIndex)
        {
            return await this.IndexInternal(toIndex, IndexType.InProgress);
        }

        public async Task<IResult> Delete(string id)
        {
            return await this.DeleteInternal(new SearchableCategory(id));
        }

        private async Task<IResult> IndexInternal(CategoryDto toIndex, IndexType indexType)
        {
            var searchableCategory = new SearchableCategory(toIndex.Id, toIndex.Name, toIndex.Description, toIndex.SortOrder);
           return await this.Index(searchableCategory, indexType);
        }
    }
}