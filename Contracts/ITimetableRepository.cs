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
        IEnumerable<Timetable> GetTimetablesByMaster(int masterId);
        IEnumerable<Timetable> GetTimetablesByMasterAndDate(int masterId, DateTime date);
        Timetable GetTimetableByMasterAndDateTime(int masterId, DateTime date, TimeSpan start, TimeSpan end);
        bool IsValid(Timetable timetable);
        void CreateTimetable(Timetable timetable);
        void UpdateTimetable(Timetable dbTimetable, Timetable timetable);
        void DeleteTimetable(Timetable timetable);
    }
}
