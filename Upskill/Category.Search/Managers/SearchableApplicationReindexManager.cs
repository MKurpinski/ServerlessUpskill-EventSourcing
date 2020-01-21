using System.Threading.Tasks;
using Category.Search.Models;
using Upskill.Infrastructure;
using Upskill.Search.Managers;
using Upskill.Search.Tables.Repositories;

namespace Category.Search.Managers
{
    public class SearchableCategoryReindexManager : IndexManager, ISearchableCategoryReindexManager
    {
        public SearchableCategoryReindexManager(ISearchableIndexRepository searchableIndexRepository, IDateTimeProvider dateTimeProvider) : base(searchableIndexRepository, dateTimeProvider)
        {
        }

        public async Task StartReindex()
        {
            await this.StartReindex<SearchableCategory>();
        }

        public async Task FinishReindexing()
        {
            await this.FinishReindexing<SearchableCategory>();
        }
    }
}