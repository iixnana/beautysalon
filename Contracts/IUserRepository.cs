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
        User GetUserByEmail(string email);
        IEnumerable<User> GetAllMasters();
        IEnumerable<User> GetAllClients();
        IEnumerable<User> GetAllAdmins();
        UserExtended GetUserWithDetails(int userId);
        void CreateUser(User user);
        void UpdateUser(User dbUser, User user);
        void DeleteUser(User user);
        //User Authenticate(string email, string password, string secret);
        bool IsValidUser(string email, string password);
    }
}
