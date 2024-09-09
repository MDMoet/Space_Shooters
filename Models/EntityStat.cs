using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity

namespace Space_Shooters.Models;

[Table("entity_stats")]
[Index("EntityId", Name = "entity_id", IsUnique = true)]
public partial class EntityStat
{
    [Key]
    [Column("entity_id", TypeName = "int(11)")]
    public int EntityId { get; set; }

    [Column("min_level", TypeName = "int(11)")]
    public int MinLevel { get; set; }

    [Column("max_level", TypeName = "int(11)")]
    public int MaxLevel { get; set; }

    [Column("health", TypeName = "int(11)")]
    public int Health { get; set; }

    [Column("base_damage", TypeName = "int(11)")]
    public int BaseDamage { get; set; }

    [Column("base_speed", TypeName = "int(11)")]
    public int BaseSpeed { get; set; }

    [Column("base_attack_speed", TypeName = "int(11)")]
    public int BaseAttackSpeed { get; set; }

    [Column("begin_spawn_wave", TypeName = "int(11)")]
    public int BeginSpawnWave { get; set; }

    [ForeignKey("EntityId")]
    [InverseProperty("EntityStat")]
    public virtual Entity Entity { get; set; } = null!;
}
