using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstBrandDto
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Updated By Cannot be Empty!")]
        public string UpdatedBy { get; set; } = string.Empty;
    }
}