using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Shared.Domain.Entities
{
    public class MstUser: BaseEntity
    {
        public string Email {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;
        public string FullName {get; set;} = string.Empty;
    }
}