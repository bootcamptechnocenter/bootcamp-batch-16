using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstTypeDto
    {
        [Required(ErrorMessage = "Id wajib diisi")]
        public int Id { get; set;}
        [Required(ErrorMessage = "BrandId wajib diisi")]
        public Int128 BrandId { get; set;}
        
        [Required(ErrorMessage = "Code wajib diisi")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name wajib diisi")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string UpdatedBy { get; set; } = "Admin";
    }
}