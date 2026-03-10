using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Shared.Entities
{
    public class PageResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public Pagination Pagination { get; set; } = new Pagination();
    }
}