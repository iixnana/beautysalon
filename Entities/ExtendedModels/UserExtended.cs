using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ExtendedModels
{
    public class UserExtended : IEntity
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
        public IEnumerable<Timetable> Timetables { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<Article> Articles { get; set; }

        public UserExtended()
        {

        }

        public UserExtended(User user)
        {
            Id = user.Id;
            CreationDate = user.CreationDate;
            UserType = user.UserType;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Password = user.Password;
            Phone = user.Phone;
            Email = user.Email;
        }

    }
}
