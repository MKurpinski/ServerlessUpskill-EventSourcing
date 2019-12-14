using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Providers;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Search.Indexer
{
    public class SearchableApplicationIndexerIndexer : BaseIndexer<Model.SearchableApplication>, ISearchableApplicationIndexer
    {
        private readonly IMapper _mapper;

        public SearchableApplicationIndexerIndexer(
            IMapper mapper,
            ILogger logger,
            ISearchIndexClientProvider searchIndexClientProvider) : base(searchIndexClientProvider, logger)
        {
            _mapper = mapper;
        }

        public async Task Index(ApplicationDto toIndex)
        {
            var searchableApplication = _mapper.Map<ApplicationDto, Model.SearchableApplication>(toIndex);
            await this.Index(searchableApplication);
        }
    }
}