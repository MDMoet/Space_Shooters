using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("user_skin")]
public partial class UserSkin
{
    [Key]
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("skin")]
    [StringLength(128)]
    public string Skin { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserSkin")]
    public virtual User User { get; set; } = null!;
}
