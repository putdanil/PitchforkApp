using NLayerApp.BLL.DTO;
using NLayerApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.BLL.Interfaces
{
    public interface IRoleService
    {
        RoleDTO GetRole(int? id);
        IEnumerable<RoleDTO> GetRoles();
        void Dispose();
    }
}
