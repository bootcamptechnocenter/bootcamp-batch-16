using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Entities;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqGetMstStockDto: ReqBaseParamDto
    {
        public int? ModelId { get; set; } 
    }
}
