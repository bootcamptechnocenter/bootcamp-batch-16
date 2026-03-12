using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateTypeDto
    {
        [Required(ErrorMessage = "BrandId Wajib Diisi!")]
        public int BrandId { get; set; } = 0;

        [Required(ErrorMessage = "Code Wajib Diisi!")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name Wajib Diisi!")]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}