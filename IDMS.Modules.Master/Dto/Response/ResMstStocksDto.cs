using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Response
{
    public class ResMstStocksDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public int ModelId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}