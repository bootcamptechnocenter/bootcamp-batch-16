using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Module.Master.Dto.Request;
using IDMS.Module.Master.Dto.Response;

namespace IDMS.Module.Master.Services
{
    public interface IMstUserService
    {
        Task CreateMstUser(ReqCreateMstUserDto dto);

        Task<ResLoginDto> Login(ReqLoginDto dto);
    }
}