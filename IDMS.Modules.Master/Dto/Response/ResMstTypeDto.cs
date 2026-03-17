using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Response
{
    public class ResMstTypeDto
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string BrandCode { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
