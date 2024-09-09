using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("username")]
    [StringLength(128)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(128)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(128)]
    public string Password { get; set; } = null!;

    [Column("admin", TypeName = "tinyint(4)")]
    public sbyte Admin { get; set; }

    [InverseProperty("User")]
    public virtual UserGameStat? UserGameStat { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<UserKeybind> UserKeybinds { get; set; } = new List<UserKeybind>();

    [InverseProperty("User")]
    public virtual UserSkin? UserSkin { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<UsersShop> UsersShops { get; set; } = new List<UsersShop>();
}
