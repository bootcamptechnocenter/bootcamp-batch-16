using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Response
{
    public class ResMstStockDto
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int JumlahStock { get; set; }
        public int Harga { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
    }
}