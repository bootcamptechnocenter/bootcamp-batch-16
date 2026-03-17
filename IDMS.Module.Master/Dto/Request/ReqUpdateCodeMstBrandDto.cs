using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Module.Master.Dto.Request
{
    public class ReqUpdateCodeMstBrandDto
    {
        [Required(ErrorMessage = "Code wajib diisi")]
        public string Code { get; set; }
    }
}