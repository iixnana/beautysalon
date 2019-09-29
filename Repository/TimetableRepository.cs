using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class TimetableRepository: RepositoryBase<Timetable>, ITimetableRepository
    {
        public TimetableRepository(RepositoryContext repositoryContext) :base(repositoryContext)
        {

        }
    }
}
