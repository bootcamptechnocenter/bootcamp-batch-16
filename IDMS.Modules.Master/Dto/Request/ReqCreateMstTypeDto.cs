using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateMstTypeDto
    {
        [Required(ErrorMessage = "BrandId wajib diisi")]
        public int BrandId { get; set;}
        
        [Required(ErrorMessage = "Code wajib diisi")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name wajib diisi")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = "Admin";
    }
}