using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstStockDto
    {
        [Required(ErrorMessage = "model must be selected")]
        [Range(1, int.MaxValue, ErrorMessage = "Model must be selected")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "Jumlah Stock must be filled")]
        public int JumlahStock { get; set; } 

        [Required(ErrorMessage = "Harga must be filled")]
        public decimal Harga { get; set; } 

        // public bool IsActive { get; set; } = true;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}