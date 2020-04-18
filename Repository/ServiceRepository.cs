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
    public class ServiceRepository: RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(RepositoryContext repositoryContext) :base(repositoryContext)
        {

        }

        public IEnumerable<Service> ServicesByReservation(int masterId)
        {
            return FindByCondition(a => a.MasterId.Equals(masterId)).ToList();
        }

        public IEnumerable<Service> GetAllServices()
        {
            return FindAll().OrderBy(x => x.Id).ToList();
        }

        [HttpGet("{id}", Name = nameof(GetServiceById))]
        public Service GetServiceById(int serviceId)
        {
            return FindByCondition(service => service.Id.Equals(serviceId)).DefaultIfEmpty(new Service()).FirstOrDefault();
        }

        public ServiceExtended GetServiceWithDetails(int id)
        {
            Service service = GetServiceById(id);
            return new ServiceExtended(service)
            {
                Reservations = RepositoryContext.Reservations.Where(x => x.ServiceId == service.Id),
                Master = RepositoryContext.Users.FirstOrDefault(x => x.Id == service.MasterId)
            };
        }

        public IEnumerable<ServiceExtended> GetServicesWithDetails()
        {
            IEnumerable<Service> services = GetAllServices();
            List<ServiceExtended> detailedServices = new List<ServiceExtended>();
            foreach (Service service in services)
            {
                detailedServices.Add(GetServiceWithDetails(service.Id));
            }
            return detailedServices;
        }

        public IEnumerable<ServiceExtended> GetServicesByMaster(int masterId)
        {
            IEnumerable<Service> services = FindByCondition(service => service.MasterId.Equals(masterId)).ToList();
            List<ServiceExtended> detailedServices = new List<ServiceExtended>();
            foreach (Service service in services)
            {
                detailedServices.Add(GetServiceWithDetails(service.Id));
            }
            return detailedServices;
        }

        public void CreateService(Service service)
        {
            Create(service);
        }


        public void UpdateService(Service dbService, Service service)
        {
            dbService.Map(service);
            Update(dbService);
        }

        public void DeleteService(Service service)
        {
            Delete(service);
        }
    }
}
