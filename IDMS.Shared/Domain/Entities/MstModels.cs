using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstModels : BaseEntity
    {
        public int TypeId { get; set; } = 0;

        public int BrandId { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; } = 0;

        public MstTypes? Type { get; set; }

        public MstBrands? Brand { get; set; }
    }
}