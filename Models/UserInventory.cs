using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity

namespace Space_Shooters.Models;

[Keyless]
[Table("user_inventory")]
[Index("ItemId", Name = "inv_items_itemid")]
[Index("UserId", Name = "inv_users_userid")]
public partial class UserInventory
{
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("item_id", TypeName = "int(11)")]
    public int ItemId { get; set; }

    [Column("level", TypeName = "int(11)")]
    public int Level { get; set; }

    [Column("amount", TypeName = "int(11)")]
    public int Amount { get; set; }

    [ForeignKey("ItemId")]
    public virtual Item Item { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
