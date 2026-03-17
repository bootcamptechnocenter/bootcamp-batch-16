using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Module.Master.Services
{
    public interface IAuthClientService
    {
        Task<(bool Success, string Token, DateTime ExpiresAt, string Message)> LoginAsync(string username, string password);
    }
}