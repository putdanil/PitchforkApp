using NLayerApp.DAL.EF;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private PitchforkContext db;
        private RoleRepository rolesRepository;
        private UserRepository usersRepository;
        private TombstoneRepository tombstonesRepository;

        public EFUnitOfWork()
        {
            db = new PitchforkContext();
        }
        public RoleRepository Roles
        {
            get
            {
                rolesRepository ??= new RoleRepository(db);
                return rolesRepository;
            }
        }

        public UserRepository Users
        {
            get
            {
                usersRepository ??= new UserRepository(db);
                return usersRepository;
            }
        }

        public TombstoneRepository Tombstones
        {
            get
            {
                tombstonesRepository ??= new TombstoneRepository(db);
                return tombstonesRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}