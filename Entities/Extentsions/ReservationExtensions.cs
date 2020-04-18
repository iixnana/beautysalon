using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.Extentsions
{
    public static class ReservationExtensions
    {
        public static void Map(this Reservation dbReservation, Reservation reservation)
        {
            dbReservation.Id = reservation.Id;
            dbReservation.MasterId = reservation.MasterId;
            dbReservation.UserId = reservation.UserId;
            dbReservation.TimeStart = reservation.TimeStart;
            dbReservation.TimeEnd = reservation.TimeEnd;
            dbReservation.ServiceId = reservation.ServiceId;
            dbReservation.TimetableId = reservation.TimetableId;
        }
    }
}
