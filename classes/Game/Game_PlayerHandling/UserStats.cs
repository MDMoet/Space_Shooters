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
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using Space_Shooters.classes.Game.Game_DataHandling;

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
            _UserModel = new()
            {
                UserStat = userStatsClass,
                UserGameStat = userGameStatsClass
            };
        }
        public static void UpdateGameStats(int userId)
        {
            using var context = new GameContext();
            
            // Read
            var userGameStats = context.UserGameStats.FirstOrDefault(ugs => ugs.UserId == userId);

            // Update
            userGameStats.Kills = _UserModel.UserGameStat.Kills;
            userGameStats.Deaths = _UserModel.UserGameStat.Deaths;
            userGameStats.DamageDone = _UserModel.UserGameStat.DamageDone;
            userGameStats.MissedShots = _UserModel.UserGameStat.MissedShots;
            userGameStats.HitShots = _UserModel.UserGameStat.HitShots;
            userGameStats.AverageAccuracy = Convert.ToInt32(AverageCalculator.AverageCalculation(userGameStats.HitShots, userGameStats.MissedShots));
            if (userGameStats.WavePr < Wave)
            {
                userGameStats.WavePr = Wave;
            }
            context.SaveChanges();
        }
    }
}
