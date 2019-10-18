using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extentsions
{
    public static class UserExtensions
    {
        public static void Map(this User dbUser, User user)
        {
            dbUser.Id = user.Id;
            dbUser.CreationDate = user.CreationDate;
            dbUser.UserType = user.UserType;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Password = user.Password;
            dbUser.Phone = user.Phone;
            dbUser.Email = user.Email;
            dbUser.Token = user.Token;
        }
    }
}
