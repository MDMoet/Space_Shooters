using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("entities")]
public partial class Entity
{
    [Key]
    [Column("entity_id", TypeName = "int(11)")]
    public int EntityId { get; set; }

    [Column("name")]
    [StringLength(128)]
    public string Name { get; set; } = null!;

    [InverseProperty("Entity")]
    public virtual EntitySkin? EntitySkin { get; set; }

    [InverseProperty("Entity")]
    public virtual EntityStat? EntityStat { get; set; }
}
