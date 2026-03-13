using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;

namespace WebApi.Modules.Master.Services
{
    public interface IAuthService
    {
        Task<ResAuthLoginDto> Login(ReqAuthLoginDto dto);
        Task<ResAuthDto> Register(ReqAuthRegisterDto dto);
    }
}