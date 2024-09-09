using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("user_actions")]
public partial class UserAction
{
    [Key]
    [Column("action_id", TypeName = "int(11)")]
    public int ActionId { get; set; }

    [Column("action")]
    [StringLength(128)]
    public string Action { get; set; } = null!;

    [InverseProperty("Action")]
    public virtual ICollection<UserKeybind> UserKeybinds { get; set; } = new List<UserKeybind>();
}
