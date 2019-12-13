using Entities.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Contracts
{
    public interface IAuthService
    {
        bool IsAuthenticated(AuthData request, out string token);
        JwtSecurityToken ReadToken(string header);
        bool IsAdmin(string header);
        bool IsClient(string header);
        bool IsMaster(string header);
        int GetId(string header);
    }
}
