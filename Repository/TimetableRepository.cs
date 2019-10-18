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
