using System;
using System.Collections.Generic;
using System.Text;
using Entities.ExtendedModels;
using Entities.Models;

namespace Contracts
{
    public interface IServiceRepository :IRepositoryBase<Service>
    {
        IEnumerable<Service> ServicesByReservation(int masterId);
        IEnumerable<Service> GetAllServices();
        Service GetServiceById(int serviceId);
        ServiceExtended GetServiceWithDetails(int id);
        IEnumerable<ServiceExtended> GetServicesWithDetails();
        IEnumerable<ServiceExtended> GetServicesByMaster(int masterId);
        void CreateService(Service service);
        void UpdateService(Service dbService, Service service);
        void DeleteService(Service service);
    }
}
