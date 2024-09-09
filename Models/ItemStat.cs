using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("item_stats")]
public partial class ItemStat
{
    [Key]
    [Column("item_id", TypeName = "int(11)")]
    public int ItemId { get; set; }

    [Column("level", TypeName = "int(11)")]
    public int Level { get; set; }

    [Column("damage", TypeName = "int(64)")]
    public int? Damage { get; set; }

    [Column("healing", TypeName = "int(64)")]
    public int? Healing { get; set; }

    [Column("durability", TypeName = "int(64)")]
    public int Durability { get; set; }

    [Column("attack_speed", TypeName = "int(64)")]
    public int AttackSpeed { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemStat")]
    public virtual Item Item { get; set; } = null!;
}
