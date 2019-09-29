using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;
using Entities.ExtendedModels;

namespace Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int userId);
        UserExtended GetUserWithDetails(int userId);
        void CreateUser(User user);
        void UpdateUser(User dbUser, User user);
        void DeleteUser(User user);
    }
}
