using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateMstStockDto
    {
        [Required(ErrorMessage = "ModelId is required")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "JumlahStock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "JumlahStock must be a non-negative integer")]
        public int JumlahStock { get; set; }

        [Required(ErrorMessage = "Harga is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Harga must be a non-negative integer")]
        public int Harga { get; set; }
    }
}