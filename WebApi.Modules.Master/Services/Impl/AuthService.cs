using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data;
using WebApi.Modules.Master.Dto.Request;
using WebApi.Modules.Master.Dto.Response;
using WebApi.Shared.Domain.Entities;

namespace WebApi.Modules.Master.Services.Impl
{
    public class AuthService(AppDbContext context, IJwtService jwtService) : IAuthService
    {
        private readonly AppDbContext _context = context;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<ResAuthDto> Register(ReqAuthRegisterDto dto)
        {
            var existingUser = await _context.MstUsers
                .Where(x => x.Email == dto.Email && x.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (existingUser != null) throw new Exception("User with this email already exists");

            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new MstUsers
            {
                Email = dto.Email,
                Password = dto.Password,
                FullName = dto.FullName,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            _context.MstUsers.Add(user);
            await _context.SaveChangesAsync();

            return new ResAuthDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };
        }

        public async Task<ResAuthLoginDto> Login(ReqAuthLoginDto dto)
        {
            var user = await _context.MstUsers
                .Where(x => x.Email == dto.Email && x.DeletedAt == null)
                .FirstOrDefaultAsync() ?? throw new Exception("Invalid email or password");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new Exception("Invalid email or password");

            ReqJwtDto jwtDto = new()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };

            return new ResAuthLoginDto
            {
                Email = user.Email,
                Token = _jwtService.GenerateToken(jwtDto)
            };
        }
    }
}