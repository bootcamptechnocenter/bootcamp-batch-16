using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateMstModelDto
    {
        [Required(ErrorMessage = "Type must be selected")]
        [Range(1, int.MaxValue, ErrorMessage = "Type must be selected")]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Code must be filled")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name must be filled")]
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; }

        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
    }
}