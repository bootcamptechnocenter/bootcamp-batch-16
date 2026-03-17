using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Response
{
    public class ResMstModelDto
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int BrandId { get; set; }
        public string TypeCode { get; set; } = string.Empty;
        // public string TypeCode { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;

        public string ModelCode { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; } = 0;
        
    }
}