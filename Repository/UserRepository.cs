using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;
using Entities.ExtendedModels;
using Microsoft.AspNetCore.Mvc;
using Entities.Extentsions;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            :base(repositoryContext)
        {

        }

        public IEnumerable<User> GetAllUsers()
        {
            return FindAll().OrderBy(x => x.Id).ToList();
        }

        [HttpGet("{id}", Name = nameof(GetUserById))]
        public User GetUserById(int userId)
        {
            return FindByCondition(user => user.Id.Equals(userId)).DefaultIfEmpty(new User()).FirstOrDefault();
        }

        public UserExtended GetUserWithDetails(int userId)
        {
            return new UserExtended(GetUserById(userId))
            {
                Reservations = RepositoryContext.Reservations.Where(a => a.UserId == userId)
            };
        }

        public void CreateUser(User user)
        {
            user.CreationDate = DateTime.UtcNow;
            Create(user);
        }

        public void UpdateUser(User dbUser, User user)
        {
            dbUser.Map(user);
            Update(dbUser);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }
    }
}
