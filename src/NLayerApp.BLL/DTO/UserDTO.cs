using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLayerApp.DAL.Entities;

namespace NLayerApp.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public bool? IsDeleted { get; set; }

        public string? Role { get; set; }
    }
}
