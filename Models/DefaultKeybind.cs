using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("default_keybinds")]
public partial class DefaultKeybind
{
    [Key]
    [Column("keybind_id", TypeName = "int(11)")]
    public int KeybindId { get; set; }

    [ForeignKey("KeybindId")]
    [InverseProperty("DefaultKeybind")]
    public virtual KeybindEnum Keybind { get; set; } = null!;
}
