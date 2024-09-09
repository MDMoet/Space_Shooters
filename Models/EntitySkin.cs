using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("entity_skin")]
public partial class EntitySkin
{
    [Key]
    [Column("entity_id", TypeName = "int(11)")]
    public int EntityId { get; set; }

    [Column("skin")]
    [StringLength(128)]
    public string Skin { get; set; } = null!;

    [ForeignKey("EntityId")]
    [InverseProperty("EntitySkin")]
    public virtual Entity Entity { get; set; } = null!;
}
