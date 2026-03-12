using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Response
{
    public class ResMstTypeDto
    {
        public int Id { get; set; }
        public int MstBrandId { get; set; }
        public string? BrandName { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}