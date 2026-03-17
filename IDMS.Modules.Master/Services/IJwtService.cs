using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;

namespace IDMS.Modules.Master.Services
{
    public interface IJwtService
    {
        ResGenerateAccessToken GenerateAccessToken(MstUsers user);
        string GenerateRefreshToken();
    }
}