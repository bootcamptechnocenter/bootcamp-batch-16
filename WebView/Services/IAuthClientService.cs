using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebView.Models;

namespace WebView.Services
{
    public interface IAuthClientService
    {
        Task<ResAuthClientDto> LoginAsync(ReqAuthLoginDto dto);
        Task<ApiClientResponse<object>> RegisterAsync(ReqAuthRegisterDto dto);
    }
}