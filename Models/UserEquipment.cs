using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity

namespace Space_Shooters.Models;

[Keyless]
[Table("user_equipment")]
[Index("ItemId", Name = "items_equip_itemid")]
[Index("UserId", Name = "users_equip_userid")]
public partial class UserEquipment
{
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("item_id", TypeName = "int(11)")]
    public int? ItemId { get; set; }

    [Column("itemslot", TypeName = "int(11)")]
    public int Itemslot { get; set; }

    [ForeignKey("ItemId")]
    public virtual Item? Item { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
