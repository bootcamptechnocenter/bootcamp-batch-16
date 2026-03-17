using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Web.Models;

namespace IDMS.Web.Services
{
     public interface IAuthClientService
    {
        Task<(bool Success, string Token, DateTime ExpiresAt, string Message)> LoginAsync(string username, string password);
        Task<(bool Success, string Message)> RegisterAsync(ReqRegisterDto req);
    }
}