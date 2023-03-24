using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Models;

[Table("Role")]
public partial class Role
{
    [Key]
    public int Id { get; set; }

    [StringLength(15)]
    public string Name { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
