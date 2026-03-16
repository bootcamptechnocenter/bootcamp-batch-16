using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;

namespace IDMS.Modules.Master.Services
{
    public interface IMstUserService
    {
        Task<ResMstUserDto?> CreateMstUser(ReqCreateMstUserDto dto);
        Task<ResLoginDto?> Login(ReqLoginDto dto);
    }
}