using System.Security.Claims;
using IDMS.Infrastructure.Data;
using IDMS.Modules.Master.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<string?> GetCurrentUserFullNameAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || user.Identity?.IsAuthenticated is false)
            {
                return null;
            }

            var fullName = user.FindFirst("UserFullName")?.Value;
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                return fullName;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                var nameFromDb = await _context.MstUsers
                    .Where(x => x.Id == userId)
                    .Select(x => x.FullName)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrWhiteSpace(nameFromDb))
                {
                    return nameFromDb;
                }
            }

            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            if (!string.IsNullOrWhiteSpace(email))
            {
                var nameFromDb = await _context.MstUsers
                    .Where(x => x.Email == email)
                    .Select(x => x.FullName)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrWhiteSpace(nameFromDb))
                {
                    return nameFromDb;
                }
            }

            return null;
        }
    }
}
