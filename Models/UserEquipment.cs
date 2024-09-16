using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class UserEquipment
{
    public int UserId { get; set; }

    public int? ItemId { get; set; }

    public int Itemslot { get; set; }

    public virtual Item? Item { get; set; }

    public virtual User User { get; set; } = null!;
}
