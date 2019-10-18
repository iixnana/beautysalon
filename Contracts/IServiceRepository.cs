using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface IServiceRepository :IRepositoryBase<Service>
    {
        IEnumerable<Service> ServicesByReservation(int masterId);
        IEnumerable<Service> GetAllServices();
        Service GetServiceById(int serviceId);
        void CreateService(Service service);
        void UpdateService(Service dbService, Service service);
        void DeleteService(Service service);
    }
}
