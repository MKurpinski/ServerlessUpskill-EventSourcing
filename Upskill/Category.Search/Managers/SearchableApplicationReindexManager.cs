using System.Threading.Tasks;
using Category.Search.Models;
using Upskill.Infrastructure;
using Upskill.Search.Enums;
using Upskill.Search.Managers;
using Upskill.Search.Providers;
using Upskill.Search.Tables.Repositories;

namespace Category.Search.Managers
{
    public class SearchableCategoryReindexManager : IndexManager, ISearchableCategoryReindexManager
    {
        public SearchableCategoryReindexManager(
            ISearchableIndexRepository searchableIndexRepository,
            IDateTimeProvider dateTimeProvider,
            ISearchServiceClientProvider searchServiceClientProvider) : base(searchableIndexRepository, dateTimeProvider, searchServiceClientProvider)
        {
        }

        public async Task StartReindex()
        {
            await this.OpenIndex<SearchableCategory>(IndexType.InProgress);
        }

        public async Task FinishReindexing()
        {
            await this.FinishReindexing<SearchableCategory>();
        }
    }
}