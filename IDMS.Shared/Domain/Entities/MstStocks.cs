using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstStocks : BaseEntity
    {
        public int ModelId { get; set; }
        public int JumlahStock { get; set; } = 0;
        public decimal Harga { get; set; } = 0.0m;
        public bool IsActive { get; set; } = true;
        public MstModels? Model { get; set; }
    }
}