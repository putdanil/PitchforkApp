using AutoMapper;
using NLayerApp.BLL.DTO;
using NLayerApp.BLL.Infrastructure;
using NLayerApp.BLL.Interfaces;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;


namespace NLayerApp.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public UserDTO GetUser(int? id)
        {
            if (id == null)
                throw new ValidationException("Не установлено id пользователя", "");
            var user = Database.Users.Get(id.Value);
            return new UserDTO
            {
                Login = user.Login,
                DateOfBirth = user.DateOfBirth,
                Id = user.Id,
                IsDeleted = user.IsDeleted,
                Name = user.Name,
                Password = user.Password,
                RoleId = user.RoleId,
                Surname = user.Surname
            };
        }
        public void CreateUser(UserDTO user)
        {
            User db_user = new()
            {
                Login = user.Login,
                DateOfBirth = user.DateOfBirth,
                Id = user.Id,
                IsDeleted = user.IsDeleted,
                Name = user.Name,
                Password = user.Password,
                RoleId = user.RoleId,
                Surname = user.Surname
            };

            Database.Users.Create(db_user);
            Database.Save();
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(Database.Users.GetAll());
        }

        (string login, string role) IUserService.FindUserByLogin(string? login)
        {
            if (login == null)
                throw new ValidationException("Не установлен login пользователя", "");
            var user = Database.Users.Find(p => p.Login == login).FirstOrDefault();
            if (user == null)
                return (null, null);
            return (user.Login, user.Role.Name);
        }

        void IUserService.Dispose()
        {
            Database.Dispose();
        }
    }
}
