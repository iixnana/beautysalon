using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class ReservationRepository: RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(RepositoryContext repositoryContext) :base(repositoryContext)
        {

        }

        public IEnumerable<Reservation> ReservationsByUser(int userId)
        {
            return FindByCondition(a => a.UserId.Equals(userId)).ToList();
        }
    }
}
