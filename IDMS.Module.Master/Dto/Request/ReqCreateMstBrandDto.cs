using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Module.Master.Dto.Request
{

    public class ReqCreateMstBrandDto
    {
        [Required(ErrorMessage = "Code wajib diisi")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "Name wajib diisi")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

    }
}