﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;
using Entities.ExtendedModels;
using Microsoft.AspNetCore.Mvc;
using Entities.Extentsions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

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

        public bool IsValidUser(string email, string password)
        {
            var user = FindByCondition(x => x.Email.Equals(email) && x.Password.Equals(password)).DefaultIfEmpty(null).FirstOrDefault();
            if (user.IsObjectNull()) return false; //not found
            return true;
        }

        //public User Authenticate(string email, string password, string secret)
        //{
        //    var user = FindByCondition(x => x.Email.Equals(email) && x.Password.Equals(password)).DefaultIfEmpty(null).FirstOrDefault();
        //    if (user.IsObjectNull()) return null; //not found

        //    //auth
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim(ClaimTypes.Name, user.Id.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(3),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    user.Token = tokenHandler.WriteToken(token);

        //    // remove password before returning
        //    user.Password = null;

        //    return user;
        //}

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
