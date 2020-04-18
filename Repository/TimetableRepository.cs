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

        public List<DateTime[]> GetTimes(int masterId, int serviceId)
        {
            TimeSpan duration = RepositoryContext.Services.FirstOrDefault(x => x.Id == serviceId).Time;
            List<DateTime[]> times = new List<DateTime[]>();
            var timetables = FindByCondition(timetable => timetable.MasterId.Equals(masterId) && timetable.Date >= DateTime.Today).ToList();
            foreach(Timetable timetable in timetables)
            {
                DateTime[] time = new DateTime[2];
                time[0] = DateTime.Parse(timetable.Date.ToString("yyyy-MM-dd") + " " + timetable.TimeStart);
                time[1] = DateTime.Parse(timetable.Date.ToString("yyyy-MM-dd") + " " + timetable.TimeEnd);
                times.Add(time);
                TimetableExtended timetableExtended = new TimetableExtended(timetable)
                {
                    Reservations = RepositoryContext.Reservations.Where(x => x.TimetableId == timetable.Id).ToList()
                };
                foreach(Reservation res in timetableExtended.Reservations)
                {
                    time = new DateTime[2];
                    DateTime[] tt = null;
                    for (int i = 0; i < times.Count; i++)
                    {
                        if(times[i][0] <= res.TimeStart && times[i][1] >= res.TimeEnd)
                        {
                            tt = times[i];
                            break;
                        }
                    }
                    if(tt != null)
                    {
                        times.Remove(tt);
                        if (tt[0] == res.TimeStart)
                        {
                            time[0] = res.TimeEnd;
                            time[1] = tt[1];
                            times.Add(time);
                        }
                        else if (tt[1] == res.TimeEnd)
                        {
                            time[0] = tt[0];
                            time[1] = res.TimeStart;
                            times.Add(time);
                        }
                        else
                        {
                            time[0] = tt[0];
                            time[1] = res.TimeStart;
                            times.Add(time);
                            time = new DateTime[2];
                            time[0] = res.TimeEnd;
                            time[1] = tt[1];
                            times.Add(time);
                        }
                    }
                }
            }
            List<DateTime[]> toBeRemoved = new List<DateTime[]>();
            List<DateTime[]> toBeAdded = new List<DateTime[]>();
            foreach (DateTime[] t in times)
            {
                var start = t[0].TimeOfDay;
                var end = t[1].TimeOfDay;
                if(end - start < duration)
                {
                    toBeRemoved.Add(t);
                }
                else if (end - start == duration)
                {
                    continue;
                }
                else if(end - start > duration)
                {
                    toBeRemoved.Add(t);
                    var a = start;
                    while (a + duration <= end)
                    {
                        var newSlotStart = a;
                        var newSlotEnd = a + duration;
                        a = newSlotEnd;
                        var time = new DateTime[2];
                        time[0] = DateTime.Parse(t[0].Date.ToString("yyyy-MM-dd") + " " + newSlotStart);
                        time[1] = DateTime.Parse(t[1].Date.ToString("yyyy-MM-dd") + " " + newSlotEnd);
                        toBeAdded.Add(time);
                    }
                }
            }
            foreach(var x in toBeRemoved)
            {
                times.Remove(x);
            }
            times.AddRange(toBeAdded);
            return times;
        }

        public bool IsValid(Timetable timetable)
        {
            if (timetable.TimeStart > timetable.TimeEnd) return false;
            var tbs = GetTimetablesByMasterAndDate(timetable.MasterId, timetable.Date);
            foreach(Timetable tb in tbs)
            {
                if ((timetable.TimeStart <= tb.TimeEnd && timetable.TimeStart >= tb.TimeStart) || 
                    (timetable.TimeEnd <= tb.TimeEnd && timetable.TimeEnd >= tb.TimeStart) ||
                    (timetable.TimeStart <= tb.TimeEnd && timetable.TimeEnd >= tb.TimeEnd) ||
                    (timetable.TimeStart <= tb.TimeStart && timetable.TimeEnd >= tb.TimeStart)) return false;
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
