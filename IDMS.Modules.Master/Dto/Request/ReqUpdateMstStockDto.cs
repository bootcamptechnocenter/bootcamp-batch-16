using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstStockDto
    {
        public int ModelId { get; set; }
        public string Color { get; set; } = string.Empty;
        public string PoliceNumber { get; set; } = string.Empty;
        public string NewUsed { get; set; } = string.Empty;
        public bool IsReady { get; set; }
        public decimal Price { get; set; }
        public string? UpdatedBy { get; set; }
    }
}