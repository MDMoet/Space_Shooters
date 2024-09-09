using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity

namespace Space_Shooters.Models;

[Table("keybind_enums")]
[Index("Keybind", "KeybindEnum1", Name = "keybind")]
public partial class KeybindEnum
{
    [Key]
    [Column("keybind_id", TypeName = "int(11)")]
    public int KeybindId { get; set; }

    [Column("keybind")]
    [StringLength(128)]
    public string Keybind { get; set; } = null!;

    [Column("keybind_enum", TypeName = "int(11)")]
    public int KeybindEnum1 { get; set; }

    [InverseProperty("Keybind")]
    public virtual DefaultKeybind? DefaultKeybind { get; set; }

    [InverseProperty("Keybind")]
    public virtual ICollection<UserKeybind> UserKeybinds { get; set; } = new List<UserKeybind>();
}
