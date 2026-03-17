using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;

namespace IDMS.Module.Master.Dto.Response
{
    public class RestMstTypeDto

    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int MstBrandId { get; set; }

        public MstBrands? Brand { get; set; } = null;

        public string MstBrandName { get; set; } = string.Empty;

    }
}