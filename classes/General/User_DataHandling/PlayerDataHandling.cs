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
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Space_Shooters.views;
using Microsoft.Identity.Client.NativeInterop;

namespace Space_Shooters.classes.General.User_DataHandling
{
    internal class PlayerDataHandling
    {
        public static void GetStatsFromDB()
        {
            try
            { 
                UserStat userStatsClass = new();
                UserGameStat userGameStatsClass = new();
                string userSkinString = "";
                using var context = new GameContext();
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
                var userSkin = context.UserSkins.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);
                if (userSkin != null)
                {
                    userSkinString = userSkin.Skin;
                }
                _UserModel = new()
                {
                    Skin = userSkinString,
                    UserStat = userStatsClass,
                    UserGameStat = userGameStatsClass
                };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void UpdateSkin(string equippedSkin)
        {
            try
            {
                using var context = new GameContext();
                // Read
                var userSkins = context.UserSkins.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);

                // Update
                userSkins.Skin = equippedSkin;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public static void UpdateDatabase()
        {
            UpdatePlayerLevel();
            UpdateGameStats();
        }
        public static void UpdateGameStats()
        {
            try
            {
                using var context = new GameContext();
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public static void UpdateStats()
        {
            try
            {
                using var context = new GameContext();
                // Read
                var userStats = context.UserStats.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);

                // Update
                userStats.LevelPoints = _UserModel.UserStat.LevelPoints;
                userStats.BaseDamage = _UserModel.UserStat.BaseDamage;
                userStats.BaseSpeed = _UserModel.UserStat.BaseSpeed;
                userStats.BaseAttackSpeed = _UserModel.UserStat.BaseAttackSpeed;
                userStats.Health = _UserModel.UserStat.Health;

                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void UpdatePlayerLevel()
        {
            try
            {
                using var context = new GameContext();
                // Read
                var userStats = context.UserStats.First(ugs => ugs.UserId == MainWindow.UserId);

                // Update
                userStats.LevelProgression = _UserModel.UserStat.LevelProgression;
                userStats.Level = _UserModel.UserStat.Level;
                userStats.LevelPoints = _UserModel.UserStat.LevelPoints;
                userStats.Health = _UserModel.UserStat.Health;

                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void RetrieveInventoryItems()
        {
            try
            {
                using var context = new GameContext();

                var userInventory = context.UserInventories
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Select(ui => new
                    {
                        ui.Item.Skin,
                        ui.Item.Name,
                        ui.ItemId,
                        ui.Amount,
                        ui.Level
                    })
                    .ToArray();

                foreach (var item in userInventory)
                {
                    if (item.ItemId != 1)
                    {
                        ListBoxItem container = new()
                        {
                            Height = 75,
                            Width = 75,
                            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                            Background = Brushes.Transparent,
                            IsHitTestVisible = false,
                            Margin = new Thickness(5, 2, 0, 3)
                        };

                        StackPanel stackPanel = new();

                        Image image = new()
                        {
                            Width = 50,
                            Height = 50,
                            Source = new BitmapImage(new Uri("..\\img\\skins\\Item_Skins\\" + item.Skin + ".png", UriKind.Relative)),
                            Margin = new Thickness(0, 2, 0, 0)
                        };

                        Viewbox viewbox = new();
                        TextBlock textBlock = new()
                        {
                            Text = item.Name,
                            FontFamily = new FontFamily("Inter"),
                            Width = 75,
                            TextAlignment = TextAlignment.Center,
                            FontSize = 10,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                            Opacity = 0.6,
                            Margin = new Thickness(0, 0, 0, 0)
                        };

                        Viewbox viewbox2 = new();
                        TextBlock textBlock2 = new()
                        {
                            Text = item.Amount.ToString() + "x",
                            FontFamily = new FontFamily("Inter"),
                            Width = 75,
                            TextAlignment = TextAlignment.Center,
                            FontSize = 8,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                            Opacity = 0.6,
                            Margin = new Thickness(0, 0, 0, 0)
                        };

                        viewbox.Child = textBlock;
                        viewbox2.Child = textBlock2;
                        stackPanel.Children.Add(image);
                        stackPanel.Children.Add(viewbox);
                        stackPanel.Children.Add(viewbox2);
                        container.Content = stackPanel;

                        views.Inventory.ItemsList.Items.Add(container);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public static void RetrieveInventoryEquipment()
        {
            try
            {
                using var context = new GameContext();

                var userInventory = context.UserEquipments
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Select(ui => new
                    {
                        ui.ItemId,
                        ui.Item.Name,
                        ui.Item.Skin,

                    })
                    .ToArray();

                foreach (var item in userInventory)
                {
                    if (item.ItemId != 1)
                    {
                        ListBoxItem container = new()
                        {
                            Height = 75,
                            Width = 75,
                            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                            Background = Brushes.Transparent,
                            IsHitTestVisible = false,
                            Margin = new Thickness(5, 2, 0, 3)
                        };

                        StackPanel stackPanel = new();

                        Image image = new()
                        {
                            Width = 50,
                            Height = 50,
                            Source = new BitmapImage(new Uri("..\\img\\skins\\Equipment_Skins\\" + item.Skin + ".png", UriKind.Relative)),
                            Margin = new Thickness(0, 2, 0, 0)
                        };

                        Viewbox viewbox = new();
                        TextBlock textBlock = new()
                        {
                            Text = item.Name,
                            FontFamily = new FontFamily("Inter"),
                            Width = 75,
                            TextAlignment = TextAlignment.Center,
                            FontSize = 10,
                            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                            Opacity = 0.6,
                            Margin = new Thickness(0, 0, 0, 0)
                        };

                        viewbox.Child = textBlock;
                        stackPanel.Children.Add(image);
                        stackPanel.Children.Add(viewbox);
                        container.Content = stackPanel;

                        views.Inventory.ItemsList.Items.Add(container);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void AddItemsToInventory(int itemId, int ItemLevel, int itemAmount)
        {
            try
            {
                using var context = new GameContext();

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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
