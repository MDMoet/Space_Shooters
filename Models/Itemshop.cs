using System;
using System.Collections.Generic;

namespace Space_Shooters.Models;

public partial class Itemshop
{
    public int ShopitemId { get; set; }

    public int ItemId { get; set; }

    public int Worth { get; set; }

    public virtual Item Item { get; set; } = null!;
}
