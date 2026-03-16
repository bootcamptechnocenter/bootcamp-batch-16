using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Web.Services
{
    public interface IAuthClientService
    {
        Task<(bool Success, string Token, DateTime ExpiresAt, string Message)> LoginAsync(string email, string password);
    }
}