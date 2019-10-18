using Contracts;
using Entities.Models;
using Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Repository
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRep;
        private readonly AppSettings _appSettings;


        public AuthService(IUserRepository service, IOptions<AppSettings> tokenManagement)
        {
            _userRep = service;
            _appSettings = tokenManagement.Value;
        }

        public bool IsAuthenticated(AuthData request, out string token)
        {
            token = string.Empty;
            if (!_userRep.IsValidUser(request.Email, request.Password)) return false;

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, request.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _appSettings.Issuer,
                _appSettings.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(_appSettings.AccessExpiration),
                signingCredentials: credentials
            );
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;

        }
    }
}
