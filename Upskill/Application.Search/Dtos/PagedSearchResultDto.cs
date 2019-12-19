using System.Collections.Generic;

namespace Application.Search.Dtos
{
    public class PagedSearchResultDto<T>
    {
        public IReadOnlyCollection<T> Items { get; }
        public long Total { get; }

        public PagedSearchResultDto(IReadOnlyCollection<T> items, long total)
        {
            Items = items;
            Total = total;
        }
    }
}
