using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Module.Master.Services.Impl
{
    public interface IJwtServices
    {
        string GenerateToken(string Id, string Email, string FullName);
    }
}