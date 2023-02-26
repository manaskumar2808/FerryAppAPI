using FerryAppApi.Repository;
using FerryAppApi.Models;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Collections.Generic;

namespace FerryAppApi.Repository {
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly IConfiguration iconfiguration;
        public JWTManagerRepository(IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
        }
        public Tokens Authenticate(ManasUser manasUser)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier as string, manasUser.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name as string, manasUser.UserName.ToString()));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, manasUser.UserName) }),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { 
                Token = tokenHandler.WriteToken(token), 
                Id = manasUser.Id, 
                UserName = manasUser.UserName 
            };
        }
    }
}