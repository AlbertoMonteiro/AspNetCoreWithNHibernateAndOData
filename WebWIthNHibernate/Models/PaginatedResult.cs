using System.Collections.Generic;

namespace WebWIthNHibernate.Models
{
    public sealed class PaginatedResult<T> 
    {
        public PaginatedResult(IEnumerable<T> items, long? count)
        {
            Items = items;
            Count = count;
        }

        public IEnumerable<T> Items { get; }
        public long? Count { get; }

    }
}
