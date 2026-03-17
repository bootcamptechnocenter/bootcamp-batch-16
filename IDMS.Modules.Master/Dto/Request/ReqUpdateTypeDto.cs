using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateTypeDto
    {
        public int? BrandId { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Updated By Cannot be Empty!")]
        public required string UpdatedBy { get; set; }
    }
}