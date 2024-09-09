using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity

namespace Space_Shooters.Models;

[PrimaryKey("UserId", "KeybindId", "ActionId")]
[Table("user_keybinds")]
[Index("ActionId", Name = "action_id")]
[Index("KeybindId", Name = "keybind_id")]
[Index("UserId", Name = "user_id")]
public partial class UserKeybind
{
    [Key]
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Key]
    [Column("keybind_id", TypeName = "int(11)")]
    public int KeybindId { get; set; }

    [Key]
    [Column("action_id", TypeName = "int(11)")]
    public int ActionId { get; set; }

    [ForeignKey("ActionId")]
    [InverseProperty("UserKeybinds")]
    public virtual UserAction Action { get; set; } = null!;

    [ForeignKey("KeybindId")]
    [InverseProperty("UserKeybinds")]
    public virtual KeybindEnum Keybind { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserKeybinds")]
    public virtual User User { get; set; } = null!;
}
