using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IAuthService
    {
        bool IsAuthenticated(AuthData request, out string token);
    }
}
