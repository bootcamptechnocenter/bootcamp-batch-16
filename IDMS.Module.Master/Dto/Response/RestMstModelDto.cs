using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;

namespace IDMS.Module.Master.Dto.Response
{
    public class RestMstModelDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int Year { get; set; }
        public int MstTypeId { get; set; }
        // public string MstTypeName { get; set; } = string.Empty;

        public MstTypes? Type { get; set; }
        // public int? MstBrandId { get; set; }
        // public string MstBrandName { get; set; } = string.Empty;
    }
}