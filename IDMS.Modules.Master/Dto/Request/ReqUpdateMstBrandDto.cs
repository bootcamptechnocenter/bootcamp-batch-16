using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstBrandDto
    {
        [Required(ErrorMessage = "Code must be filled")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "Name must be filled")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } 
        public string UpdatedBy { get; set; } = string.Empty;

    }
}