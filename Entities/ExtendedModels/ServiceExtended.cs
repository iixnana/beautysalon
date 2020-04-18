using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.ExtendedModels
{
    public class ServiceExtended : IEntity
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
        public User Master { get; set; }

        public ServiceExtended()
        {

        }

        public ServiceExtended(Service service)
        {
            Id = service.Id;
            MasterId = service.MasterId;
            Title = service.Title;
            Description = service.Description;
            Time = service.Time;
        }
    }
}
