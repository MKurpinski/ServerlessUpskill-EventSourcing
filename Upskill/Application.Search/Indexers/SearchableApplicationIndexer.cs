using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Models;
using Application.Search.Providers;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Search.Indexers
{
    public class SearchableApplicationIndexer : BaseIndexer<SearchableApplication>, ISearchableApplicationIndexer
    {
        private readonly IMapper _mapper;

        public SearchableApplicationIndexer(
            IMapper mapper,
            ILogger logger,
            ISearchIndexClientProvider searchIndexClientProvider) : base(searchIndexClientProvider, logger)
        {
            _mapper = mapper;
        }

        public async Task Index(ApplicationDto toIndex)
        {
            var searchableApplication = _mapper.Map<ApplicationDto, SearchableApplication>(toIndex);
            await this.Index(searchableApplication);
        }
    }
}