using AutoMapper;
using NLayerApp.BLL.DTO;
using NLayerApp.BLL.Infrastructure;
using NLayerApp.BLL.Interfaces;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;


namespace NLayerApp.BLL.Services
{
    public class RoleService : IRoleService
    {
        IUnitOfWork Database { get; set; }

        public RoleService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public RoleDTO GetRole(int? id)
        {
            if (id == null)
                throw new ValidationException("Не установлено id", "");
            var role = Database.Roles.Get(id.Value);
            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Role, RoleDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Role>, List<RoleDTO>>(Database.Roles.GetAll());
        }

        void IRoleService.Dispose()
        {
            Database.Dispose();
        }
    }
}
