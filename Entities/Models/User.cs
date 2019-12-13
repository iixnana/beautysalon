using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("users")]
    public class User : IEntity
    {
        [Key]
        //[Column("Id")] - this maps right column in the database, so the name of property can be different
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string UserType { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(30, ErrorMessage = "Name can't be longer than 30 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(30, ErrorMessage = "Name can't be longer than 30 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


    }

    public static class Role
    {
        public const string Client = "Client";
        public const string Master = "Master";
        public const string Admin = "Admin";
        public const string ClientMasterAdmin = "Client, Master, Admin";
        public const string ClientMaster = "Client, Master";
        public const string ClientAdmin = "Client, Admin";
        public const string MasterAdmin = "Master, Admin";
    }
}
