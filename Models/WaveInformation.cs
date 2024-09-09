using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute; // Alias to resolve ambiguity


namespace Space_Shooters.Models;

[Table("wave_information")]
[Index("WaveNumber", Name = "wave_number", IsUnique = true)]
public partial class WaveInformation
{
    [Key]
    [Column("wave_id", TypeName = "int(11)")]
    public int WaveId { get; set; }

    [Column("wave_number", TypeName = "int(11)")]
    public int WaveNumber { get; set; }

    [Column("boss_wave", TypeName = "tinyint(4)")]
    public sbyte BossWave { get; set; }

    [Column("entity_amount", TypeName = "int(11)")]
    public int EntityAmount { get; set; }
}
