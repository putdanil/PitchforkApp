using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserRepository Users { get; }
        RoleRepository Roles { get; }
        TombstoneRepository Tombstones { get; }
        void Save();
    }
}