using System.ComponentModel.DataAnnotations;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateMstModelDto
    {
        [Required(ErrorMessage = "TypeId wajib diisi")]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Code wajib diisi")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name wajib diisi")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year wajib diisi")]
        public int Year { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
