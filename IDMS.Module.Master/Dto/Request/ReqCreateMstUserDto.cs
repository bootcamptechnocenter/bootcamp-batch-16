using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Module.Master.Dto.Request
{
    public class ReqCreateMstUserDto
    {
        [Required(ErrorMessage = "FullName wajib diisi")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password wajib diisi")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Email tidak valid")]
        public string Email { get; set; } = string.Empty;
    }
}