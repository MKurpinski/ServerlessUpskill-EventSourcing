using Application.Search.Models;
using Upskill.ReindexGuards;

namespace Application.Core.Guards
{
    public interface IApplicationQueueingEventGuard : IQueueingEventGuard<SearchableApplication, Aggregates.Application>
    {
    }
}
