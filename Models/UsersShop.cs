using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity

namespace Space_Shooters.Models;

[Table("users_shop")]
[Index("UserId", Name = "shop_user_id")]
[Index("UserItemId", Name = "shop_user_item_id")]
public partial class UsersShop
{
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Key]
    [Column("users_shopitem_id", TypeName = "int(11)")]
    public int UsersShopitemId { get; set; }

    [Column("user_item_id", TypeName = "int(11)")]
    public int UserItemId { get; set; }

    [Column("worth", TypeName = "int(11)")]
    public int Worth { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UsersShops")]
    public virtual User User { get; set; } = null!;
}
