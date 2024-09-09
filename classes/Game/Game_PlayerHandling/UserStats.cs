using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Space_Shooters.Models;
using Space_Shooters.Context;
using Space_Shooters.classes.Game.Game_VariableHandling;

namespace Space_Shooters.classes.Game.Game_PlayerHandling
{
    internal class User
    {
        public static void GetStatsFromDB(int userId)
        {
            UserStat userStatsClass = new();
            UserGameStat userGameStatsClass = new();
            using var context = new GameContext();
            // Assuming you have a primary key 'Id' in your UserStats table
            var userStats = context.UserStats.FirstOrDefault(us => us.UserId == userId);
            if (userStats != null)
            {
                userStatsClass.Level = userStats.Level;
                userStatsClass.LevelProgression = userStats.LevelProgression;
                userStatsClass.LevelPoints = userStats.LevelPoints;
                userStatsClass.Health = userStats.Health;
                userStatsClass.BaseDamage = userStats.BaseDamage;
                userStatsClass.BaseAttackSpeed = userStats.BaseAttackSpeed;
            }

            var userGameStats = context.UserGameStats.FirstOrDefault(ugs => ugs.UserId == userId);
            if (userGameStats != null)
            {
                userGameStatsClass.WavePr = userGameStats.WavePr;
                userGameStatsClass.Deaths = userGameStats.Deaths;
                userGameStatsClass.Kills = userGameStats.Kills;
                userGameStatsClass.DamageDone = userGameStats.DamageDone;
                userGameStatsClass.MissedShots = userGameStats.MissedShots;
                userGameStatsClass.HitShots = userGameStats.HitShots;
                userGameStatsClass.AverageAccuracy = userGameStats.AverageAccuracy;
            }
            PassableVariables.userStat = userStatsClass;
            PassableVariables.userGameStat = userGameStatsClass;
        }
    }
}
