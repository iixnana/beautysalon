using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.ExtendedModels
{
    public class TimetableExtended
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }

        public TimetableExtended()
        {

        }

        public TimetableExtended(Timetable timetable)
        {
            Id = timetable.Id;
            MasterId = timetable.MasterId;
            Date = timetable.Date;
            TimeStart = timetable.TimeStart;
            TimeEnd = timetable.TimeEnd;
        }
    }
}
