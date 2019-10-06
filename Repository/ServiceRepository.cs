using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class ServiceRepository: RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(RepositoryContext repositoryContext) :base(repositoryContext)
        {

        }

        public IEnumerable<Service> ServicesByReservation(int masterId)
        {
            return FindByCondition(a => a.MasterId.Equals(masterId)).ToList();
        }
    }
}
