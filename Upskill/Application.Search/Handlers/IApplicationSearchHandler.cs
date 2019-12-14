using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Model;
using Application.Search.Providers;
using Application.Search.Queries;
using AutoMapper;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Application.Search.Handlers
{
    public interface IApplicationSearchHandler
    {
        Task<PagedSearchResultDto<SimpleApplicationDto>> Search(SimpleApplicationSearchQuery query);
    }

    public class ApplicationSearchHandler : IApplicationSearchHandler
    {
        private readonly ISearchIndexClientProvider _searchIndexClientProvider;
        private readonly IMapper _mapper;

        public ApplicationSearchHandler(
            ISearchIndexClientProvider searchIndexClientProvider,
            IMapper mapper)
        {
            _searchIndexClientProvider = searchIndexClientProvider;
            _mapper = mapper;
        }

        public async Task<PagedSearchResultDto<SimpleApplicationDto>> Search(SimpleApplicationSearchQuery query)
        {
            var client = await _searchIndexClientProvider.Get<SearchableApplication>();

            var searchResults = await client.Documents.SearchAsync<SearchableApplication>(query.SearchPhrase,
                new SearchParameters(
                    includeTotalResultCount: true,
                    skip: query.Skip,
                    top: query.Take));

            var mappedResults = _mapper.Map<IReadOnlyCollection<SearchableApplication>, IReadOnlyCollection<SimpleApplicationDto>>(searchResults.Results.Select(x => x.Document).ToList());
            var result = new PagedSearchResultDto<SimpleApplicationDto>(mappedResults, searchResults.Count.Value);
            return result;
        }
    }
}
