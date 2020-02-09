using Application.Search.Models;
using Upskill.ReindexGuards;
using Upskill.Search.Managers;

namespace Application.Core.Guards
{
    public class ApplicationQueueingEventGuard : QueueingEventGuard<SearchableApplication, Aggregates.Application>, IApplicationQueueingEventGuard
    {
        public ApplicationQueueingEventGuard(IIndexManager indexManager) : base(indexManager)
        {
        }
    }
}