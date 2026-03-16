using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;

namespace WebView.Services
{
    public interface IAuthClientService
    {
        Task<ResAuthClientDto> LoginAsync(ReqAuthClientLoginDto dto);
    }
}