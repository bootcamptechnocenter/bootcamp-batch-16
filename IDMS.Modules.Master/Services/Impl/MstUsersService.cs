using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IDMS.Infrastructure.Data;
using IDMS.Modules.Master.Dto.Request;
using IDMS.Modules.Master.Dto.Response;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Modules.Master.Services.Impl
{
    public class MstUsersService : IMstUsersService
    {
        private readonly AppDbContext _dbContext;
        private readonly IJwtService _jwtService;

        public MstUsersService(AppDbContext dbContext, IJwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        public async Task<ResLoginUser?> LoginUser(ReqLoginUser dto)
        {
            var user = await _dbContext.MstUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user is null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var expiredAt = accessToken.ExpiredAt;

            return new ResLoginUser
            {
                AccessToken = accessToken.AccessToken,
                RefreshToken = refreshToken,
                ExpiredAt = expiredAt
            };
        }

        public async Task<bool> RegisterUser(ReqRegisterUser dto)
        {
            var isUserExist = await _dbContext.MstUsers.AnyAsync(x => x.UserName == dto.UserName || x.Email == dto.Email);
            if (isUserExist)
            {
                throw new ArgumentException("User already exists");
            }

            var newUser = new MstUsers
            {
                UserName = dto.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Email = dto.Email
            };

            await _dbContext.MstUsers.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}