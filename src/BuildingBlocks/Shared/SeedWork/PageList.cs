using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class PageList<T> : List<T>
    {
        private MetaData _metaData { get; }

        public MetaData GetMetaData()
        {
            return _metaData;
        }
        public PageList(IEnumerable<T> items, long totalItems, int pageNumber, int pageSize)
        {
            _metaData = new MetaData()
            {
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages= (int)Math.Ceiling(totalItems / (double) pageSize)
            };
            AddRange(items);
        }
    }
}
