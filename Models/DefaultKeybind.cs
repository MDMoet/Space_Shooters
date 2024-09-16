using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class DefaultKeybind
{
    public int KeybindId { get; set; }

    public virtual KeybindEnum Keybind { get; set; } = null!;
}
