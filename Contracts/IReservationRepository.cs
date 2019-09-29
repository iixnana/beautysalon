using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface IReservationRepository : IRepositoryBase<Reservation>
    {
        IEnumerable<Reservation> ReservationsByUser(int userId);
    }
}
