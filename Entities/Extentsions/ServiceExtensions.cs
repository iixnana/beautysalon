using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.Extentsions
{
    public static class ServiceExtensions
    {
        public static void Map(this Service dbService, Service service)
        {
            dbService.Id = service.Id;
            dbService.MasterId = service.MasterId;
            dbService.Title = service.Title;
            dbService.Time = service.Time;
            dbService.Description = service.Description;
        }
    }
}
