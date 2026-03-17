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
        public int JumlahStock { get; set; }
        public decimal Harga { get; set; }

        public MstModels? Model { get; set; }
    }
}