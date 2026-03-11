using System.ComponentModel.DataAnnotations;

namespace WebApi.Modules.Master.Dto.Request
{
    public class ReqMstModelDto
    {
        [Required(ErrorMessage = "Type Id is required")]
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Year is required")]
        public int Year { get; set; }
        public bool IsActive { get; set; } = true;
    }
}