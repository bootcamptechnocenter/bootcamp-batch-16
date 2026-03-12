using System.ComponentModel.DataAnnotations;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstTypeDto
    {
        [Required(ErrorMessage = "BrandId wajib diisi")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Code wajib diisi")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name wajib diisi")]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public string? UpdatedBy { get; set; }
    }
}
