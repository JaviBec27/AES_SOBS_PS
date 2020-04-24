using AES_SOBS_PS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AES_SOBS_PS.Services.SecurityServices
{
    public class TokenService
    {
        private readonly IConfiguration configuration;
        private readonly int expirationDays;

        public TokenService(IConfiguration configuration, int expirationDays)
        {
            this.configuration = configuration;
            this.expirationDays = expirationDays;
        }

        public void BuildToken(UserInfo userInfo)
        {
            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
               new Claim("keyValue", "AESTRASNOCHO"),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["KeySecret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(expirationDays);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
                );

            userInfo.Token = new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
