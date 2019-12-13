using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.ExtendedModels
{
    public class ReservationExtended : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MasterId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public Boolean ApprovedByUser { get; set; }
        public string Status { get; set; }

        public IEnumerable<Service> Services { get; set; }

        public ReservationExtended()
        {

        }

        public ReservationExtended(Reservation reservation)
        {
            Id = reservation.Id;
            UserId = reservation.UserId;
            MasterId = reservation.MasterId;
            TimeStart = reservation.TimeStart;
            TimeEnd = reservation.TimeEnd;
        }

    }
}
