using System.ComponentModel.DataAnnotations;

namespace WebApi.Modules.Master.Dto.Request
{
    public class ReqMstBrandDto
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}