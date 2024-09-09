using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity

namespace Space_Shooters.Models;

[Table("itemshop")]
[Index("ItemId", Name = "itemshop_item_id")]
public partial class Itemshop
{
    [Key]
    [Column("shopitem_id", TypeName = "int(11)")]
    public int ShopitemId { get; set; }

    [Column("item_id", TypeName = "int(11)")]
    public int ItemId { get; set; }

    [Column("worth", TypeName = "int(11)")]
    public int Worth { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("Itemshops")]
    public virtual Item Item { get; set; } = null!;
}
