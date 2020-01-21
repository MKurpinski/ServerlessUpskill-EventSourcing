using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Upskill.Search.Enums;
using Upskill.Search.Indexers;
using Upskill.Search.Providers;

namespace Application.Search.Indexers
{
    public class SearchableApplicationIndexer : BaseIndexer<SearchableApplication>, ISearchableApplicationIndexer
    {
        private readonly IMapper _mapper;

        public SearchableApplicationIndexer(
            IMapper mapper,
            ILogger<SearchableApplicationIndexer> logger,
            ISearchIndexClientProvider searchIndexClientProvider) : base(searchIndexClientProvider, logger)
        {
            _mapper = mapper;
        }

        public async Task Index(ApplicationDto toIndex)
        {
            await this.IndexInternal(toIndex, IndexType.Active);
        }

        public async Task Reindex(ApplicationDto toIndex)
        {
            await this.IndexInternal(toIndex, IndexType.InProgress);
        }

        public async Task BuildNewIndex()
        {
            await this.OpenNewIndex();
        }

        private async Task IndexInternal(ApplicationDto toIndex, IndexType indexType)
        {
            var searchableApplication = _mapper.Map<ApplicationDto, SearchableApplication>(toIndex);
            await this.Index(searchableApplication, indexType);
        }
    }
}