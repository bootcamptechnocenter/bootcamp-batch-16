using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;

namespace IDMS.Modules.Master.Services
{
    public interface IMstUserService
    {
        Task<bool> CreateUserAsync(ReqCreateMstUserDto req);
        Task<ResLoginDto> Login(ReqLoginDto req);
    }
}