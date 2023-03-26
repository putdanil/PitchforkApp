using NLayerApp.BLL.DTO;
using NLayerApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.BLL.Interfaces
{
    public interface IUserService
    { 
        UserDTO GetUser(int? id);
        (string? login, string? role) FindUserByLogin(string? login);
        IEnumerable<UserDTO> GetUsers();
        void CreateUser(UserDTO user);
        void Dispose();
    }
}
