using System;
using System.Collections.Generic;
using System.Text;
using Entities.ExtendedModels;
using Entities.Models;

namespace Contracts
{
    public interface IReservationRepository : IRepositoryBase<Reservation>
    {
        IEnumerable<Reservation> ReservationsByUser(int userId);
        IEnumerable<Reservation> GetAllReservations();
        Reservation GetReservationById(int userId);
        //ReservationExtended GetReservationWithDetails(int userId);
        void CreateReservation(Reservation reservation);
        void UpdateReservation(Reservation dbReservation, Reservation reservation);
        void DeleteReservation(Reservation reservation);
    }
}
