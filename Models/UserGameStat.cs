using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Space_Shooters.Models;

[Table("user_game_stats")]
public partial class UserGameStat
{
    [Key]
    [Column("user_id", TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column("wave_pr", TypeName = "int(11)")]
    public int WavePr { get; set; }

    [Column("deaths", TypeName = "int(11)")]
    public int Deaths { get; set; }

    [Column("kills", TypeName = "int(11)")]
    public int Kills { get; set; }

    [Column("damage_done", TypeName = "int(11)")]
    public int DamageDone { get; set; }

    [Column("missed_shots", TypeName = "int(11)")]
    public int MissedShots { get; set; }

    [Column("hit_shots", TypeName = "int(11)")]
    public int HitShots { get; set; }

    [Column("average_accuracy", TypeName = "int(11)")]
    public int AverageAccuracy { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserGameStat")]
    public virtual User User { get; set; } = null!;
}
