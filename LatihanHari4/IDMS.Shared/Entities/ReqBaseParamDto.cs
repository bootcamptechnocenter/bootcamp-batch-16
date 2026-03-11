using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Shared.Entities
{
    public class ReqBaseParamDto
    {
        [FromQuery(Name = "pageIndex")]

        public int Page {get; set;} = 1;
        public int Limit {get; set;} = 10;
        public string? Search {get; set;}
    }
}