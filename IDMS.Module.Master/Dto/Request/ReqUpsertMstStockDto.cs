using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Module.Master.Dto.Request
{
    public class ReqUpsertMstStockDto
    {
        [Required(ErrorMessage = "Model Id wajib disini")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "Total Stock wajib diisi")]
        [Range(1, int.MaxValue, ErrorMessage = "Total Stock harus berupa angka positif")]
        public int TotalStock { get; set; }


        [Required(ErrorMessage = "Price wajib diisi")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price harus berupa angka positif")]
        public decimal Price { get; set; }

        public bool isAccumulate { get; set; } = false;
    }
}