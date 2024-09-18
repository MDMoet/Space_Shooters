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

namespace Space_Shooters.classes.Game.Game_DataHandling
{
    internal class User
    {
        public static void GetStatsFromDB()
        {
            UserStat userStatsClass = new();
            UserGameStat userGameStatsClass = new();
            using var context = new Context.GameContext();
            // Assuming you have a primary key 'Id' in your UserStats table
            var userStats = context.UserStats.FirstOrDefault(us => us.UserId == MainWindow.UserId);
            if (userStats != null)
            {
                userStatsClass.Level = userStats.Level;
                userStatsClass.LevelProgression = userStats.LevelProgression;
                userStatsClass.LevelPoints = userStats.LevelPoints;
                userStatsClass.Health = userStats.Health;
                userStatsClass.BaseDamage = userStats.BaseDamage;
                userStatsClass.BaseSpeed = userStats.BaseSpeed;
                userStatsClass.BaseAttackSpeed = userStats.BaseAttackSpeed;
            }

            var userGameStats = context.UserGameStats.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);
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
        public static void UpdateDatabase()
        {
            UpdatePlayerLevel();
            UpdateGameStats();
        }
        public static void UpdateGameStats()
        {
            using var context = new Context.GameContext();
            // Read
            var userGameStats = context.UserGameStats.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);

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
        public static void UpdatePlayerLevel()
        {
            using var context = new Context.GameContext();
            // Read
            var userStats = context.UserStats.First(ugs => ugs.UserId == MainWindow.UserId);

            // Update
            userStats.LevelProgression = _UserModel.UserStat.LevelProgression;
            userStats.Level = _UserModel.UserStat.Level;
            userStats.LevelPoints = _UserModel.UserStat.LevelPoints;
            userStats.Health = _UserModel.UserStat.Health;

            context.SaveChanges();
        }
        public static void AddItemsToInventory(int itemId, int ItemLevel, int itemAmount)
        {
            using var context = new Context.GameContext();

            // Read existing inventory
            var userInventory = context.UserInventories
                .Where(ui => ui.UserId == MainWindow.UserId)
                .Where(ui => ui.ItemId == itemId)
                .FirstOrDefault();

            if (userInventory == null)
            {
                // Insert new entry using raw SQL
                context.Database.ExecuteSqlRaw(
                    "INSERT INTO user_inventory (user_id, item_id, level, amount) VALUES ({0}, {1}, {2}, {3})",
                    MainWindow.UserId, itemId, ItemLevel, itemAmount);
            }
            else
            {
                // Update existing record
                context.Database.ExecuteSqlRaw(
                    "UPDATE user_inventory SET amount = {0} WHERE user_id = {1} AND item_id = {2}",
                    userInventory.Amount + itemAmount, MainWindow.UserId, itemId);
            }
        }
    }
}
