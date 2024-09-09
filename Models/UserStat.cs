using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity

namespace Space_Shooters.Models;

[Keyless]
[Table("user_stats")]
[Index("UserId", Name = "user_id", IsUnique = true)]
public partial class UserStat
{
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("level", TypeName = "int(11)")]
    public int Level { get; set; }

    [Column("level_progression", TypeName = "int(11)")]
    public int LevelProgression { get; set; }

    [Column("level_points", TypeName = "int(11)")]
    public int LevelPoints { get; set; }

    [Column("health", TypeName = "int(128)")]
    public int Health { get; set; }

    [Column("base_damage", TypeName = "int(11)")]
    public int BaseDamage { get; set; }

    [Column("base_attack_speed", TypeName = "int(11)")]
    public int BaseAttackSpeed { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
