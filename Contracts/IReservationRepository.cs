using System;
using System.Collections.Generic;
using System.Text;
using Entities.ExtendedModels;
using Entities.Models;

namespace Contracts
{
    public interface IReservationRepository : IRepositoryBase<Reservation>
    {
        IEnumerable<Reservation> GetReservationsByUser(int userId);
        IEnumerable<Reservation> ReservationsByTimetable(DateTime date, TimeSpan start, TimeSpan end);
        IEnumerable<ReservationExtended> GetReservationsByTimetable(int id);
        IEnumerable<Reservation> GetAllReservations();
        IEnumerable<Reservation> GetReservationsByMaster(int masterId);
        IEnumerable<Reservation> GetReservationsByMasterAndDate(int masterId, DateTime date);
        Reservation GetReservationById(int id);
        ReservationExtended GetReservationWithDetails(int id);
        IEnumerable<ReservationExtended> GetReservationsWithDetails();
        IEnumerable<ReservationExtended> GetReservationsWithDetails(int userId);
        bool IsValid(int masterId, DateTime date, TimeSpan start, TimeSpan end);
        //ReservationExtended GetReservationWithDetails(int userId);
        void CreateReservation(Reservation reservation);
        void UpdateReservation(Reservation dbReservation, Reservation reservation);
        void DeleteReservation(Reservation reservation);
    }
}
