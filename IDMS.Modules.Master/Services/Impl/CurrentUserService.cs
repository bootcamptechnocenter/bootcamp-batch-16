using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDMS.Modules.Master.Services.Impl
{
    public class CurrentUserService : ICurrentUserService
{
    public Task<string> GetCurrentUserFullNameAsync()
    {
        return Task.FromResult("Admin");
    }
}
}