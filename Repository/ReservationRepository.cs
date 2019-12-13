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
    public class ReservationRepository: RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(RepositoryContext repositoryContext) :base(repositoryContext)
        {

        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return FindAll().OrderBy(x => x.Id).ToList();
        }

        public IEnumerable<Reservation> GetReservationsByUser(int userId)
        {
            return FindByCondition(a => a.UserId.Equals(userId)).ToList();
        }

        public IEnumerable<Reservation> ReservationsByTimetable(DateTime date, TimeSpan start, TimeSpan end)
        {
            return FindByCondition(a => a.TimeStart.Date.Equals(date) && a.TimeStart.TimeOfDay >= start && a.TimeEnd.TimeOfDay <= end && a.Status != "Approved").ToList();
        }

        public IEnumerable<Reservation> GetReservationsByMaster(int masterId)
        {
            return FindByCondition(a => a.MasterId.Equals(masterId)).ToList();
        }

        public IEnumerable<Reservation> GetReservationsByMasterAndDate(int masterId, DateTime date)
        {
            return FindByCondition(a => a.MasterId.Equals(masterId) && a.TimeStart.Date == date).ToList();
        }

        [HttpGet("{id}", Name = nameof(GetReservationById))]
        public Reservation GetReservationById(int reservationId)
        {
            return FindByCondition(reservation => reservation.Id.Equals(reservationId)).DefaultIfEmpty(new Reservation()).FirstOrDefault();
        }

        public bool IsValid(int masterId, DateTime date, TimeSpan start, TimeSpan end)
        {
            if (start > end) return false;
            var reservations = GetReservationsByMasterAndDate(masterId, date);
            foreach (Reservation r in reservations)
            {
                if ((start > r.TimeStart.TimeOfDay && start < r.TimeEnd.TimeOfDay) || 
                    (end > r.TimeStart.TimeOfDay && end < r.TimeEnd.TimeOfDay) ||
                    (start < r.TimeStart.TimeOfDay && end > r.TimeStart.TimeOfDay) ||
                    (start > r.TimeEnd.TimeOfDay && end < r.TimeEnd.TimeOfDay)) return false;
            }
            return true;
        }

        public void CreateReservation(Reservation reservation)
        {
            Create(reservation);
        }

        //public void CreateReservation(Reservation reservation, IEnumerable<Service> services)
        //{
        //    Create(reservation);
        //    foreach(Service service in services)
        //    {
               
        //    }
        //}

        public void UpdateReservation(Reservation dbReservation, Reservation reservation)
        {
            reservation.Id = dbReservation.Id;
            dbReservation.Map(reservation);
            Update(dbReservation);
        }

        public void DeleteReservation(Reservation reservation)
        {
            Delete(reservation);
        }
    }
}
