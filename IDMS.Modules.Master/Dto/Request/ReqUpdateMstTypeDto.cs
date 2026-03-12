using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Request
{
    public class ReqUpdateMstTypeDto
    {
        // MstBrandId is required to update type, because we need to know which brand the type belongs to
        public int MstBrandId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        // created by in body request
        public string UpdatedBy { get; set; } = string.Empty;
    }
}