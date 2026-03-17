using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstStocks: BaseEntity
    {
        
        public int Model_Id { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Jumlah stock tidak boleh negatif")]
        public int JumlahStock { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Harga tidak boleh negatif")]
        public decimal Harga { get; set; }

        public int ModelId { get; set; }   // ✅ sesuai convention

        public MstModels Model { get; set; } // ✅ singular
        // public bool IsActive { get; set; } = true;

        public MstModels? Models { get; set; }
    }
}