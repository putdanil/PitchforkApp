using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    public string Name { get; set; } = null!;

    [StringLength(30)]
    public string Surname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    [StringLength(30)]
    public string Login { get; set; } = null!;

    [StringLength(50)]
    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role? Role { get; set; }
}
