using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstStock: BaseEntity
    {
        public int ModelId { get; set; }
        public int StockAmount { get; set; }
        public int Price { get; set; }

        public MstModels? Model { get; set; }
    }
}