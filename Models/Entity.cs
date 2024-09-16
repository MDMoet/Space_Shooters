using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class Entity
{
    public int EntityId { get; set; }

    public string Name { get; set; } = null!;

    public int SpawnWave { get; set; }

    public virtual EntitySkin? EntitySkin { get; set; }

    public virtual EntityStat? EntityStat { get; set; }
}
