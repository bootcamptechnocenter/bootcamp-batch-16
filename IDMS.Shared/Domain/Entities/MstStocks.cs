using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstStocks : BaseEntity
    {
        public int TotalStock { get; set; }

        public decimal Price { get; set; }
        public int MstModelId { get; set; }
        public MstModels? Model { get; set; } = null;

    }
}