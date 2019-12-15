using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Models;
using Application.Search.Providers;
using Application.Search.Queries;
using AutoMapper;
using Microsoft.Azure.Search.Models;
using Upskill.Results;
using Upskill.Results.Implementation;

namespace Application.Search.Handlers
{
    public class ApplicationSearchHandler : BaseSearchHandler<SearchableApplication>, IApplicationSearchHandler
    {
        private readonly IMapper _mapper;

        public ApplicationSearchHandler(
            ISearchIndexClientProvider searchIndexClientProvider,
            IMapper mapper) : base(searchIndexClientProvider)
        {
            _mapper = mapper;
        }

        public async Task<PagedSearchResultDto<SimpleApplicationDto>> Search(SimpleApplicationSearchQuery query)
        {

            var searchParameters = new SearchParameters
            {
                IncludeTotalResultCount = true,
                Skip = query.Skip,
                Top = query.Take,
            };

            var searchResults = await this.WildcardSearch(query.SearchPhrase, searchParameters);

            var mappedResults = _mapper.Map<IReadOnlyCollection<SearchableApplication>, IReadOnlyCollection<SimpleApplicationDto>>(searchResults.Results.Select(x => x.Document).ToList());
            var result = new PagedSearchResultDto<SimpleApplicationDto>(mappedResults, searchResults.Count.Value);
            return result;
        }

        public async Task<IDataResult<ApplicationDto>> GetById(GetApplicationByIdQuery query)
        {
            var searchResult = await this.GetById(query.Id);

            var matchingApplication = searchResult.Results.FirstOrDefault()?.Document;

            if (matchingApplication == null)
            {
                return new FailedDataResult<ApplicationDto>();
            }

            var mapped = _mapper.Map<SearchableApplication, ApplicationDto>(matchingApplication);

            return new SuccessfulDataResult<ApplicationDto>(mapped);
        }
    }
}