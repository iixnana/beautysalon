using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        ITimetableRepository Timetable { get; }
        IArticleRepository Article { get; }
        IServiceRepository Service { get; }
        IReservationRepository Reservation { get; }
        void Save();
    }
}
