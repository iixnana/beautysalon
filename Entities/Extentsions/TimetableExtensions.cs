using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extentsions
{
    public static class TimetableExtensions
    {
        public static void Map(this Timetable dbtimetable, Timetable timetable)
        {
            dbtimetable.Id = timetable.Id;
            dbtimetable.MasterId = timetable.MasterId;
            dbtimetable.Date = timetable.Date;
            dbtimetable.TimeStart = timetable.TimeStart;
            dbtimetable.TimeEnd = timetable.TimeEnd;
        }
    }
}
