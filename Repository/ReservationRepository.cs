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

        public IEnumerable<Reservation> ReservationsByUser(int userId)
        {
            return FindByCondition(a => a.UserId.Equals(userId)).ToList();
        }

        [HttpGet("{id}", Name = nameof(GetReservationById))]
        public Reservation GetReservationById(int reservationId)
        {
            return FindByCondition(reservation => reservation.Id.Equals(reservationId)).DefaultIfEmpty(new Reservation()).FirstOrDefault();
        }

        public void CreateReservation(Reservation reservation)
        {
            Create(reservation);
        }

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
