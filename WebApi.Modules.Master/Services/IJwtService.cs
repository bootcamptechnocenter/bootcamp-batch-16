using WebApi.Modules.Master.Dto.Request;

namespace WebApi.Modules.Master.Services
{
    public interface IJwtService
    {
        string GenerateToken(ReqJwtDto dto);
    }
}