using Category.Search.Models;
using Upskill.ReindexGuards;
using Upskill.Search.Managers;

namespace Category.Core.Guards
{
    public class CategoryQueueingEventGuard : QueueingEventGuard<SearchableCategory, Aggregates.Category>, ICategoryQueueingEventGuard
    {
        public CategoryQueueingEventGuard(IIndexManager indexManager) : base(indexManager)
        {
        }
    }
}