using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using IDMS.Module.Master.Dto.Request;
using IDMS.Shared.Domain.Entities;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using IDMS.Module.Master.Dto.Response;

namespace IDMS.Module.Master.Services.Impl
{
    public class MstUserService : IMstUserService
    {
        private readonly AppDbContext _context;
        private readonly IJwtServices _jwtService;

        public MstUserService(AppDbContext context, IJwtServices jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }


        public async Task CreateMstUser(ReqCreateMstUserDto dto)
        {
            if (await _context.MstUsers.AnyAsync(u => u.Email == dto.Email))
            {
                throw new Exception("Email already exists.");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var entity = new MstUser
            {
                FullName = dto.FullName,
                Password = hashedPassword,
                Email = dto.Email,
                CreatedAt = DateTime.Now,
                CreatedBy = "system"
            };

            _context.MstUsers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ResLoginDto> Login(ReqLoginDto dto)
        {
            var user = await _context.MstUsers.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new Exception("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.FullName);

            return new ResLoginDto
            {
                Token = token,
                Email = user.Email
            };
        }
    }
}