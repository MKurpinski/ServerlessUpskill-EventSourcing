using Category.Search.Models;
using Upskill.ReindexGuards;

namespace Category.Core.Guards
{
    public interface ICategoryQueueingEventGuard : IQueueingEventGuard<SearchableCategory, Aggregates.Category>
    {
    }
}
