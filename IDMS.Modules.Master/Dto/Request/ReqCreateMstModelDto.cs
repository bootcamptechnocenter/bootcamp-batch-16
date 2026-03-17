using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateMstModelDto
    {
        [Required(ErrorMessage = "Type Id wajib dimasukan")]
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Code wajib dimasukan")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name wajib dimasukan")]
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }
        public bool IsActive { get; set; } = true;
    }
}