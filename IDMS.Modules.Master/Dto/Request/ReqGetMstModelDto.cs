using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqGetMstModelDto : ReqBaseParamDto
    {
        public int? TypeId { get; set; }
    }
}