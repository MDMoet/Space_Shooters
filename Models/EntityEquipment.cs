using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity


namespace Space_Shooters.Models;

[Keyless]
[Table("entity_equipment")]
[Index("EntityId", Name = "equip_entities_entityid")]
[Index("ItemId", Name = "equip_items_itemid")]
public partial class EntityEquipment
{
    [Column("entity_id", TypeName = "int(11)")]
    public int EntityId { get; set; }

    [Column("item_id", TypeName = "int(11)")]
    public int ItemId { get; set; }

    [ForeignKey("EntityId")]
    public virtual Entity Entity { get; set; } = null!;

    [ForeignKey("ItemId")]
    public virtual Item Item { get; set; } = null!;
}
