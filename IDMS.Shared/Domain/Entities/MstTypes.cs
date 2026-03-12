using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstTypes: BaseEntity
    {
        public int MstBrandId { get; set; }
        public string? Code { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public MstBrands Brands { get; set; }
    }
}