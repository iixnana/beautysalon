using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;
using Entities.ExtendedModels;
using Microsoft.AspNetCore.Mvc;
using Entities.Extentsions;

namespace Repository
{
    public class TimetableRepository: RepositoryBase<Timetable>, ITimetableRepository
    {
        public TimetableRepository(RepositoryContext repositoryContext) :base(repositoryContext)
        {

        }

        public IEnumerable<Timetable> GetAllTimetables()
        {
            return FindAll().OrderBy(x => x.Id).ToList();
        }

        [HttpGet("{id}", Name = nameof(GetTimetableById))]
        public Timetable GetTimetableById(int timetableId)
        {
            return FindByCondition(timetable => timetable.Id.Equals(timetableId)).DefaultIfEmpty(new Timetable()).FirstOrDefault();
        }

        public IEnumerable<Timetable> GetTimetablesByMaster(int masterId)
        {
            return FindByCondition(timetable => timetable.MasterId.Equals(masterId)).ToList();
        }

        public Timetable GetTimetableByMasterAndDateTime(int masterId, DateTime date, TimeSpan start, TimeSpan end)
        {
            IEnumerable<Timetable> timetables = GetTimetablesByMasterAndDate(masterId, date);
            foreach (Timetable tb in timetables)
            {
                if (tb.TimeStart <= start && tb.TimeEnd >= end) return tb;
            }
            return null;
        }

        public IEnumerable<Timetable> GetTimetablesByMasterAndDate(int masterId, DateTime date)
        {
            return FindByCondition(timetable => timetable.MasterId.Equals(masterId) && timetable.Date.Equals(date.Date)).ToList();
        }

        public bool IsValid(Timetable timetable)
        {
            if (timetable.TimeStart > timetable.TimeEnd) return false;
            var tbs = GetTimetablesByMasterAndDate(timetable.MasterId, timetable.Date);
            foreach(Timetable tb in tbs)
            {
                if ((timetable.TimeStart < tb.TimeEnd && timetable.TimeStart > tb.TimeStart) || 
                    (timetable.TimeEnd < tb.TimeEnd && timetable.TimeEnd > tb.TimeStart) ||
                    (timetable.TimeStart < tb.TimeEnd && timetable.TimeEnd > tb.TimeEnd) ||
                    (timetable.TimeStart < tb.TimeStart && timetable.TimeEnd > tb.TimeStart)) return false;
            }
            return true;
        }

        public void CreateTimetable(Timetable timetable)
        {
            Create(timetable);
        }

        public void UpdateTimetable(Timetable dbTimetable, Timetable timetable)
        {

            dbTimetable.Map(timetable);
            Update(dbTimetable);
        }

        public void DeleteTimetable(Timetable timetable)
        {
            Delete(timetable);
        }
    }
}
