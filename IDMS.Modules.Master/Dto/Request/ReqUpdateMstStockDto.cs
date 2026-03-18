using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstStockDto
    {
        [Required(ErrorMessage = "Model must be selected")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "Price must be filled")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity must be filled")]
        public int Qty { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }
}