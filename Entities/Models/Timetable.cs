using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("timetables")]
    public class Timetable : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MasterId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime TimeStart { get; set; }

        [Required]
        public DateTime TimeEnd { get; set; }
    }
}
