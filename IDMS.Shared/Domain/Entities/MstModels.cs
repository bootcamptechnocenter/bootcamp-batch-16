using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstModels : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int Year { get; set; }

        public bool IsActive { get; set; } = true;

        public MstTypes Type { get; set; } = null!;
        public int MstTypeId { get; set; }
    }
}