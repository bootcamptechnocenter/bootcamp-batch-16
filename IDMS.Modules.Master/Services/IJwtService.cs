using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Services
{
    public interface IJwtService
    {
        string GenerateToken(string Id, string Email, string FullName);
    }
}