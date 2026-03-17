using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Dto.Response
{
    public class ResGenerateAccessToken
    {
        public required string AccessToken { get; set; }
        public required DateTime ExpiredAt { get; set; }
    }
}