using System.ComponentModel.DataAnnotations;

namespace WebApi.Modules.Master.Dto.Request
{
    public class ReqAuthLoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}