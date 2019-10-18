using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface ITimetableRepository : IRepositoryBase<Timetable>
    {
        IEnumerable<Timetable> GetAllTimetables();
        Timetable GetTimetableById(int timetableId);
        void CreateTimetable(Timetable timetable);
        void UpdateTimetable(Timetable dbTimetable, Timetable timetable);
        void DeleteTimetable(Timetable timetable);
    }
}
