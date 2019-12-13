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
    public class ReservationsHasServicesRepository : RepositoryBase<ReservationsHasServices>
    {
        public ReservationsHasServicesRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }


    }
}
