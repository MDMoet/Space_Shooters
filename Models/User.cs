using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public sbyte Admin { get; set; }

    public virtual UserGameStat? UserGameStat { get; set; }

    public virtual ICollection<UserKeybind> UserKeybinds { get; set; } = new List<UserKeybind>();

    public virtual UserSkin? UserSkin { get; set; }

    public virtual UserStat? UserStat { get; set; }

    public virtual ICollection<UsersShop> UsersShops { get; set; } = new List<UsersShop>();
}
