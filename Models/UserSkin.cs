using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserSkin
{
    public int UserId { get; set; }

    public string Skin { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
