using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Models;
using Application.Search.Queries;
using Application.Storage.Blobs.Providers;
using Application.Storage.Constants;
using AutoMapper;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Search.Handlers;
using Upskill.Search.Options;
using Upskill.Search.Providers;

namespace Application.Search.Handlers
{
    public class ApplicationSearchHandler : BaseSearchHandler<SearchableApplication>, IApplicationSearchHandler
    {
        private readonly IMapper _mapper;
        private readonly SearchOptions _searchOptions;
        private readonly ISharedAccessSignatureProvider _sharedAccessSignatureProvider;

        public ApplicationSearchHandler(
            ISearchIndexClientProvider searchIndexClientProvider,
            IMapper mapper,
            ISharedAccessSignatureProvider sharedAccessSignatureProvider,
            IOptions<SearchOptions> searchOptionsAccessor) : base(searchIndexClientProvider)
        {
            _mapper = mapper;
            _sharedAccessSignatureProvider = sharedAccessSignatureProvider;
            _searchOptions = searchOptionsAccessor.Value;
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

            var validFor = TimeSpan.FromHours(_searchOptions.SharedAccessSignatureLifetimeInHours);
            var photoToken = _sharedAccessSignatureProvider.GetContainerSasUri(FileStore.PhotosContainer, validFor);
            var cvToken = _sharedAccessSignatureProvider.GetContainerSasUri(FileStore.CvsContainer, validFor);

            var results = mappedResults.Select(x => this.EnrichDtoWithTokens(x, cvToken, photoToken)).ToList();

            var result = new PagedSearchResultDto<SimpleApplicationDto>(results, searchResults.Count.Value);
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

            var validFor = TimeSpan.FromHours(_searchOptions.SharedAccessSignatureLifetimeInHours);
            var photoToken = _sharedAccessSignatureProvider.GetContainerSasUri(FileStore.PhotosContainer, validFor);
            var cvToken = _sharedAccessSignatureProvider.GetContainerSasUri(FileStore.CvsContainer, validFor);

            return new SuccessfulDataResult<ApplicationDto>(this.EnrichDtoWithTokens(mapped, cvToken, photoToken));
        }

        public async Task<IEnumerable<ApplicationDto>> GetByCategory(GetApplicationsByCategoryQuery query)
        {
            var searchResults = await this.GetByField(nameof(SearchableApplication.Category), query.CategoryName);

            var mappedResults = _mapper.Map<IReadOnlyCollection<SearchableApplication>, IReadOnlyCollection<ApplicationDto>>(searchResults.Results.Select(x => x.Document).ToList());

            return mappedResults;
        }


        private T EnrichDtoWithTokens<T>(T dto, string cvToken, string photoToken) where T: SimpleApplicationDto
        {
            dto.CvUri = $"{dto.CvUri}{cvToken}";
            dto.PhotoUri = $"{dto.PhotoUri}{photoToken}";
            return dto;
        }
    }
}