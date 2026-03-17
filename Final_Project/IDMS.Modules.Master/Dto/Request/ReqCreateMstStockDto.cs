using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateMstStockDto
    {
        [Required(ErrorMessage = "Model must be selected")]
        [Range(1, int.MaxValue, ErrorMessage = "Model must be selected")]
        public int ModelId { get; set; }
        
        [Required(ErrorMessage = "Stock must be filled")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be positive number")]
        public int JumlahStock { get; set; }
        
        [Required(ErrorMessage = "Price must be filled")]
        [Range(1, long.MaxValue, ErrorMessage = "Price must be positive number")]
        public long Price { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}