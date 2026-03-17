using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;

namespace IDMS.Modules.Master.Services
{
    public interface IMstUsersService
    {
        public Task<bool> RegisterUser(ReqRegisterUser dto);
        public Task<ResLoginUser?> LoginUser(ReqLoginUser dto);
    }
}