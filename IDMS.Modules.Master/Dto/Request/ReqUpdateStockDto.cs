using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateStockDto
    {
        [Required(ErrorMessage = "Brand Wajib Diisi!")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Type Wajib Diisi!")]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Model Wajib Diisi!")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "Price Wajib Diisi!")]
        public int Price { get; set; }

        public int Quantity { get; set; } = 0;

        public string UpdatedBy { get; set; } = "System";
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}