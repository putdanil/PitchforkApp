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
    public class RoleDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
