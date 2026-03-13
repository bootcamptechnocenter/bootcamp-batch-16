using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using WebApi.Modules.Master.Dto.Request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Modules.Master.Services.Impl
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(ReqJwtDto dto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, dto.Id.ToString()),
                new Claim(ClaimTypes.Email, dto.Email),
                new Claim("UserFullName", dto.FullName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiredMinutesStr = _configuration["Jwt:ExpireInMinutes"];
            var expiredMinutes = string.IsNullOrEmpty(expiredMinutesStr) ? 60 : int.Parse(expiredMinutesStr);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiredMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}