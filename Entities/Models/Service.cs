using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("services")]
    public class Service : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MasterId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public TimeSpan Time { get; set; }
    }
}
