using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserAction
{
    public int ActionId { get; set; }

    public string Action { get; set; } = null!;

    public virtual ICollection<UserKeybind> UserKeybinds { get; set; } = new List<UserKeybind>();
}
