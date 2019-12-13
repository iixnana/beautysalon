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
            dbUser.Id = dbUser.Id;
            dbUser.CreationDate = dbUser.CreationDate;
            dbUser.UserType = dbUser.UserType;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Password = user.Password;
            dbUser.Phone = user.Phone;
            dbUser.Email = user.Email;
        }
    }
}
