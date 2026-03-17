using System.ComponentModel.DataAnnotations;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstStockDto
    {
        [Required(ErrorMessage = "ModelId wajib diisi")]
        public int ModelId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "JumlahStock tidak boleh negatif")]
        public int JumlahStock { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Harga tidak boleh negatif")]
        public decimal Harga { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
