using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Shared.Entities
{
    public class Pagination
    {
        public int CurrentPage {get; set;};
        public int Limit {get; set;};
        public int TotalItems {get; set;};
        public int TotalPages {get; set;};
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;
    }
}