using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("items")]
public partial class Item
{
    [Key]
    [Column("item_id", TypeName = "int(11)")]
    public int ItemId { get; set; }

    [Column("name")]
    [StringLength(256)]
    public string Name { get; set; } = null!;

    [Column("item_type")]
    [StringLength(128)]
    public string ItemType { get; set; } = null!;

    [Column("required_level", TypeName = "int(128)")]
    public int RequiredLevel { get; set; }

    [Column("skin")]
    [StringLength(128)]
    public string Skin { get; set; } = null!;

    [Column("worth", TypeName = "int(11)")]
    public int Worth { get; set; }

    [InverseProperty("Item")]
    public virtual ItemStat? ItemStat { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<Itemshop> Itemshops { get; set; } = new List<Itemshop>();
}
