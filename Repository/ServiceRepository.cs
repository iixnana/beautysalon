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
