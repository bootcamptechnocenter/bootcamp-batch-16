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

        [Required(ErrorMessage = "Jumlah Stock must be filled")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int JumlahStock { get; set; }

        [Required(ErrorMessage = "Harga must be filled")]
        [Range(0, double.MaxValue, ErrorMessage = "Harga cannot be negative")]
        public decimal Harga { get; set; }

        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; } = string.Empty;
    }
}