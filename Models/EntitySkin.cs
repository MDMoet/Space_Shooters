using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class EntitySkin
{
    public int EntityId { get; set; }

    public string Skin { get; set; } = null!;

    public virtual Entity Entity { get; set; } = null!;
}
