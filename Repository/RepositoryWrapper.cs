using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Helpers;
using Microsoft.Extensions.Options;
using System.IO;

namespace Repository
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private RepositoryContext _repoContext;
        private IUserRepository _user;
        private ITimetableRepository _timetable;
        private IArticleRepository _article;
        private IServiceRepository _service;
        private IReservationRepository _reservation;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }

                return _user;
            }
        }

        public ITimetableRepository Timetable
        {
            get
            {
                if (_timetable == null)
                {
                    _timetable = new TimetableRepository(_repoContext);
                }

                return _timetable;
            }
        }

        public IArticleRepository Article
        {
            get
            {
                if (_article == null)
                {
                    _article = new ArticleRepository(_repoContext);
                }

                return _article;
            }
        }


        public IServiceRepository Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new ServiceRepository(_repoContext);
                }

                return _service;
            }
        }

        public IReservationRepository Reservation
        {
            get
            {
                if (_reservation == null)
                {
                    _reservation = new ReservationRepository(_repoContext);
                }

                return _reservation;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
