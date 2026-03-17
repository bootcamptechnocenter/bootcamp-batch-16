using System;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Modules.Master.Services.Impl
{
    public class MstUserService : IMstUserService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ICurrentUserService _currentUserService;

        public MstUserService(AppDbContext context, IJwtService jwtService, ICurrentUserService currentUserService)
        {
            _context = context;
            _jwtService = jwtService;
            _currentUserService = currentUserService;
        }

        public async Task<ResMstUserDto?> CreateMstUser(ReqCreateMstUserDto dto)
        {
            var email = dto.Email.Trim();
            var actor = await _currentUserService.GetCurrentUserFullNameAsync();

            var isEmailExists = await _context.MstUsers.AnyAsync(x => x.Email == email);
            if (isEmailExists)
            {
                return null;
            }

            var user = new MstUsers
            {
                Email = email,
                Password = HashPassword(dto.Password),
                FullName = dto.FullName,
                CreatedBy = string.IsNullOrWhiteSpace(actor) ? "Admin" : actor,
                CreatedAt = DateTime.Now
            };

            _context.MstUsers.Add(user);
            var result = await _context.SaveChangesAsync();

            return result > 0 ? new ResMstUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            } : null;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<ResLoginDto?> Login(ReqLoginDto dto)
        {
            var email = dto.Email.Trim();
            var user = await _context.MstUsers.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new ArgumentException("Invalid email or password");
            }

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.FullName);

            return new ResLoginDto
            {
                Email = user.Email,
                Token = token
            };
        }
    }
}