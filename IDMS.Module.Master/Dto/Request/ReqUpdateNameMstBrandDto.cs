using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Module.Master.Dto.Request
{

    public class ReqUpdateNameMstBrandDto
    {
        [Required(ErrorMessage = "Name wajib diisi")]
        public string Name { get; set; } = string.Empty;
    }
}