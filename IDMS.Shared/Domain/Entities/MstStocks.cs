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
        public string? Color { get; set; }
        public string? PoliceNumber { get; set; }
        public string? NewUsed { get; set; }
        public bool IsReady { get; set; }
        public decimal Price { get; set; }

        public virtual MstModels? Model { get; set; }
    }
}
