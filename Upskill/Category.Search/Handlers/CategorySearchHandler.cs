using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Category.Search.Dtos;
using Category.Search.Models;
using Category.Search.Queries;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Search.Dtos;
using Upskill.Search.Handlers;
using Upskill.Search.Options;
using Upskill.Search.Providers;

namespace Category.Search.Handlers
{
    public class CategorySearchHandler : BaseSearchHandler<SearchableCategory>, ICategorySearchHandler
    {
        private readonly SearchOptions _searchOptions;

        public CategorySearchHandler(
            ISearchIndexClientProvider searchIndexClientProvider,
            IOptions<SearchOptions> searchOptionsAccessor) : base(searchIndexClientProvider)
        {
            _searchOptions = searchOptionsAccessor.Value;
        }

        public async Task<PagedSearchResultDto<CategoryDto>> Get(GetCategoriesQuery query)
        {
            var searchParameters = new SearchParameters
            {
                IncludeTotalResultCount = true,
                Top = query.Take,
                Skip = query.Skip,
            };

            var searchResults = await this.WildcardSearch("*", searchParameters);
            var mappedResults = searchResults.Results.Select(x =>
                new CategoryDto(x.Document.Id, x.Document.Name, x.Document.Description, x.Document.SortOrder)).ToList();

            return new PagedSearchResultDto<CategoryDto>(mappedResults, searchResults.Count.Value);
        }

        public async Task<IDataResult<CategoryDto>> GetById(GetCategoryByIdQuery query)
        {
            var searchResults = await this.GetByField(nameof(SearchableCategory.Id), query.Id);
            return this.HandleSingleResult(searchResults);
        }

        public async Task<IDataResult<CategoryDto>> GetByName(GetCategoryByNameQuery query)
        {
            var searchResults = await this.GetByField(nameof(SearchableCategory.Name), query.Name);
            return this.HandleSingleResult(searchResults);
        }

        private IDataResult<CategoryDto> HandleSingleResult(DocumentSearchResult<SearchableCategory> searchResults)
        {
            var category = searchResults.Results.Select(x => x.Document).FirstOrDefault();
            if (category == null)
            {
                return new FailedDataResult<CategoryDto>();
            }

            var dto = new CategoryDto(category.Id, category.Name, category.Description, category.SortOrder);
            return new SuccessfulDataResult<CategoryDto>(dto);
        }
    }
}