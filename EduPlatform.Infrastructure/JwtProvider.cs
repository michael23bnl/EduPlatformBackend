using EduPlatform.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using EduPlatform.Core.Abstractions;

namespace EduPlatform.Infrastructure {
    public class JwtProvider : IJwtProvider{

        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options) { 
            _options = options.Value;
        }

        public string GenerateToken(UserModel userModel) {
            Claim[] claims = [
                new (CustomClaims.UserId, userModel.Id.ToString())
                ];
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
