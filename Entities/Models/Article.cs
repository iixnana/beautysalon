using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("articles")]
    public class Article : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime PublishingTime { get; set; }

        [Required]
        public string Title { get; set; }

        public string Picture { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
