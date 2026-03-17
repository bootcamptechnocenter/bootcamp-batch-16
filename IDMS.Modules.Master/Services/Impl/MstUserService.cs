using System;
using System.Collections.Generic;
using System.Linq;
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
        public MstUserService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        public async Task<bool> CreateUserAsync(ReqCreateMstUserDto req)
        {
            bool isEmailExist = _context.MstUsers.AnyAsync(u => u.Email == req.Email).Result;
            if (isEmailExist)            {
                throw new ArgumentException("Email already exists.");
            }
            var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);
            var user = new MstUser
            {
                Email = req.Email,
                Password = encryptedPassword,
                FullName = req.FullName
            };

            _context.MstUsers.Add(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ResLoginDto> Login(ReqLoginDto req)
        {
            var user = await _context.MstUsers.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            {
                throw new ArgumentException("Invalid email or password.");
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