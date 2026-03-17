using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqCreateMstModelDto
    {
        [Required(ErrorMessage = "TypeId is required")]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
    }
}