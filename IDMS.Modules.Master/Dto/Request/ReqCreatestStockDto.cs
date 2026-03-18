using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateMstStockDto
    {
        public int ModelId { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}