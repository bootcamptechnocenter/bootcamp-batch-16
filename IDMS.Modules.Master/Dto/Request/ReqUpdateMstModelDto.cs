using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstModelDto
    {
        [Required(ErrorMessage = "Type must be selected")]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Code must be filled")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name must be filled")]
        public string Name { get; set; } = string.Empty;

        public int Year { get; set; }
        public bool IsActive { get; set; } = true;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}