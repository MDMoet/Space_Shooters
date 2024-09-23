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
using Space_Shooters.classes.Game.Game_VariableHandling;
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Space_Shooters.views;
using Microsoft.Identity.Client.NativeInterop;
using Microsoft.VisualBasic.ApplicationServices;
using Azure.Core;
using Space_Shooters.Context;
using System.Windows.Markup.Localizer;
using System.Windows.Input;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Runtime.CompilerServices;
using Microsoft.Web.WebView2.Core;
using Space_Shooters.classes.ItemShop;
using Space_Shooters.classes.UserShop;
using Space_Shooters.classes.General.Shop_DataHandling;
using System.IO;
using System.Security.Cryptography;

namespace Space_Shooters.classes.General.User_DataHandling
{
    internal class PlayerDataHandling
    {
        static bool ran = false;
        internal static Dictionary<int, Tuple<Image, OutlinedTextControl>> EquipmentSlotMap = [];
        internal static Tuple<int, int, int, int> PreviousBoosts;
        public static void GetStatsFromDB()
        {
            try
            {
                UserStat userStatsClass = new();
                UserGameStat userGameStatsClass = new();
                string userSkinString = "";
                int userSkinId = 0;
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
                var userSkin = context.UserSkins
                    .First(ugs => ugs.UserId == MainWindow.UserId);
                if (userSkin != null)
                {
                    userSkinId = userSkin.SkinId;

                    userSkinString = context.Skins
                        .Where(us => us.SkinId == userSkinId)
                        .Select(us => us.Skin1)
                        .FirstOrDefault();
                }
                _UserModel = new()
                {
                    OwnedUserSkins = [],
                    LockedUserSkins = [],
                    Skin = userSkinString,
                    SkinId = userSkinId,
                    UserStat = userStatsClass,
                    UserGameStat = userGameStatsClass
                };
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        public static void CheckPurchasedSkins()
        {
            try
            {
                _UserModel.OwnedUserSkins.Clear();
                _UserModel.LockedUserSkins.Clear();
                using var context = new GameContext();
                // Read
                var userInventorySkins = context.UserSkinsInventories
                    .Where(uis => uis.UserId == MainWindow.UserId)
                    .Select(ui => new
                    {
                        Skin = ui.Skin.Skin1,
                        ui.SkinId,
                        ui.Purchased
                    })
                    .ToList();

                foreach (var skin in userInventorySkins)
                {
                    if (skin.Purchased == 1)
                    {
                        _UserModel.OwnedUserSkins.Add(skin.Skin, skin.SkinId);
                    }
                    else
                    {
                        _UserModel.LockedUserSkins.Add(skin.Skin, skin.SkinId);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }

        }
        public static void UpdateSkin(int equippedSkin)
        {
            try
            {
                using var context = new GameContext();
                // Read
                var userSkins = context.UserSkins.FirstOrDefault(ugs => ugs.UserId == MainWindow.UserId);

                // Update
                userSkins.SkinId = equippedSkin;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
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
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }

        }
        public static void UpdateStats()
        {
            try
            {
                RemoveTempBoost();

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
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        public static void UpdatePlayerLevel()
        {
            try
            {
                RemoveTempBoost();

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
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        /*
        *                  ITEM HANDLING
        */
        public static ListBox RetrieveInventoryItems()
        {
            try
            {
                using var context = new GameContext();

                var userInventory = context.UserItemInventories
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Select(ui => new
                    {
                        ui.Item.Skin,
                        ui.Item.Name,
                        ui.ItemId,
                        ui.Item.Worth,
                        ui.Amount
                    })
                    .ToList();

                ListBox listBox = new()
                {
                    Name = "lbItemInventory",
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0)
                };
                listBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
                listBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                ItemsPanelTemplate itemsPanelTemplate = new();
                FrameworkElementFactory wrapPanelFactory = new(typeof(WrapPanel));
                wrapPanelFactory.SetValue(WrapPanel.OrientationProperty, Orientation.Horizontal);
                itemsPanelTemplate.VisualTree = wrapPanelFactory;
                listBox.ItemsPanel = itemsPanelTemplate;

                // Create a custom style for ListBoxItem
                Style listBoxItemStyle = new(typeof(ListBoxItem));

                // Set the ControlTemplate to remove the highlight
                ControlTemplate template = new(typeof(ListBoxItem));
                FrameworkElementFactory borderFactory = new(typeof(Border));
                borderFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
                template.VisualTree = borderFactory;

                listBoxItemStyle.Setters.Add(new Setter(Control.TemplateProperty, template));

                // Apply the custom style to the ListBox
                listBox.ItemContainerStyle = listBoxItemStyle;

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
                            IsHitTestVisible = true,
                            Margin = new Thickness(5, 2, 0, 3)
                        };

                        StackPanel stackPanel = new()
                        {
                            Tag = $"{item.ItemId},{0},{item.Worth}"
                        };

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
                            FontSize = 9,
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
                        stackPanel.MouseLeftButtonDown += InventorySelect;
                        container.Content = stackPanel;

                        listBox.Items.Add(container);
                    }
                }
                return listBox;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
                return null;
            }

        }

        public static void AddItemsToInventory(int itemId, int itemAmount)
        {
            try
            {
                using var context = new GameContext();

                // Read existing inventory
                var userInventory = context.UserItemInventories
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Where(ui => ui.ItemId == itemId)
                    .FirstOrDefault();

                if (userInventory == null)
                {
                    // Insert new entry using raw SQL
                    context.Database.ExecuteSqlRaw(
                        "INSERT INTO user_item_inventory (user_id, item_id, amount) VALUES ({0}, {1}, {2})",
                        MainWindow.UserId, itemId, itemAmount);
                }
                else
                {
                    // Update existing record
                    context.Database.ExecuteSqlRaw(
                        "UPDATE user_item_inventory SET amount = {0} WHERE user_id = {1} AND item_id = {2}",
                        userInventory.Amount + itemAmount, MainWindow.UserId, itemId);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        public static void RemoveItemsFromInventory(int itemId, int itemAmount)
        {
            try
            {
                using var context = new GameContext();

                // Read existing inventory
                var userInventory = context.UserItemInventories
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Where(ui => ui.ItemId == itemId)
                    .FirstOrDefault();

                if (userInventory.Amount - itemAmount != 0)
                {
                    // Update existing record
                    context.Database.ExecuteSqlRaw(
                        "UPDATE user_item_inventory SET amount = {0} WHERE user_id = {1} AND item_id = {2}",
                        userInventory.Amount - itemAmount, MainWindow.UserId, itemId);
                }
                else
                {
                    // Delete existing record
                    context.Database.ExecuteSqlRaw(
                        "DELETE FROM user_item_inventory WHERE user_id = {0} AND item_id = {1}",
                        MainWindow.UserId, itemId);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        /*
         *                  EQUIPMENT HANDLING
         */
        public static void AddEquipmentToInventory(int id, int amount)
        {
            try
            {
                using var context = new GameContext();

                // Read existing inventory
                var userInventory = context.UserEquipmentInventories
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Where(ui => ui.EquipmentId == id)
                    .FirstOrDefault();

                if (userInventory == null)
                {
                    // Insert new entry using raw SQL
                    context.Database.ExecuteSqlRaw(
                        "INSERT INTO user_equipment_inventory (user_id, equipment_id, amount) VALUES ({0}, {1}, {2})",
                        MainWindow.UserId, id, amount);
                }
                else
                {
                    // Update existing record
                    context.Database.ExecuteSqlRaw(
                        "UPDATE user_equipment_inventory SET amount = {0} WHERE user_id = {1} AND equipment_id = {2}",
                        userInventory.Amount + amount, MainWindow.UserId, id);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        public static void RemoveEquipmentFromInventory(int id, int amount)
        {
            try
            {
                using var context = new GameContext();

                // Read existing inventory
                var userInventory = context.UserEquipmentInventories
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Where(ui => ui.EquipmentId == id)
                    .FirstOrDefault();

                if (userInventory.Amount - amount > 0)
                {
                    // Update existing record
                    context.Database.ExecuteSqlRaw(
                        "UPDATE user_equipment_inventory SET amount = {0} WHERE user_id = {1} AND equipment_id = {2}",
                        userInventory.Amount - amount, MainWindow.UserId, id);
                }
                else
                {
                    // Delete existing record
                    context.Database.ExecuteSqlRaw(
                        "DELETE FROM user_equipment_inventory WHERE user_id = {0} AND equipment_id = {1}",
                        MainWindow.UserId, id);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        public static ListBox RetrieveInventoryEquipment()
        {
            try
            {
                using var context = new GameContext();

                var userInventory = context.UserEquipmentInventories
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Select(ui => new
                    {
                        ui.EquipmentId,
                        ui.Equipment.Name,
                        ui.Equipment.Skin,
                        ui.Equipment.Worth,
                        ui.Amount

                    })
                    .ToArray();

                ListBox listBox = new()
                {
                    Name = "lbItemInventory",
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0)
                };
                listBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
                listBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                ItemsPanelTemplate itemsPanelTemplate = new();
                FrameworkElementFactory wrapPanelFactory = new(typeof(WrapPanel));
                wrapPanelFactory.SetValue(WrapPanel.OrientationProperty, Orientation.Horizontal);
                itemsPanelTemplate.VisualTree = wrapPanelFactory;
                listBox.ItemsPanel = itemsPanelTemplate;

                // Create a custom style for ListBoxItem
                Style listBoxItemStyle = new(typeof(ListBoxItem));

                // Set the ControlTemplate to remove the highlight
                ControlTemplate template = new(typeof(ListBoxItem));
                FrameworkElementFactory borderFactory = new(typeof(Border));
                borderFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
                template.VisualTree = borderFactory;

                listBoxItemStyle.Setters.Add(new Setter(Control.TemplateProperty, template));

                // Apply the custom style to the ListBox
                listBox.ItemContainerStyle = listBoxItemStyle;

                foreach (var equipment in userInventory)
                {
                    ListBoxItem container = new()
                    {
                        Height = 75,
                        Width = 75,
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                        Background = Brushes.Transparent,
                        Cursor = Cursors.Hand,
                        IsHitTestVisible = true,
                        Margin = new Thickness(5, 2, 0, 3),
                    };

                    StackPanel stackPanel = new()
                    {
                        Tag = $"{equipment.EquipmentId},{1},{equipment.Worth}"
                    };

                    Image image = new()
                    {
                        Width = 50,
                        Height = 50,
                        Source = new BitmapImage(new Uri("\\..\\img\\skins\\Equipment_Skins\\" + equipment.Skin + ".png", UriKind.Relative)),
                        Margin = new Thickness(0, 2, 0, 0)
                    };

                    Viewbox viewbox = new();
                    TextBlock textBlock = new()
                    {
                        Text = equipment.Name,
                        FontFamily = new FontFamily("Inter"),
                        Width = 75,
                        TextAlignment = TextAlignment.Center,
                        FontSize = 9,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                        Opacity = 0.6,
                        Margin = new Thickness(0, 0, 0, 0)
                    };

                    Viewbox viewbox2 = new();
                    TextBlock textBlock2 = new()
                    {
                        Text = equipment.Amount.ToString() + "x",
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
                    stackPanel.MouseLeftButtonDown += InventorySelect;
                    container.Content = stackPanel;
                    listBox.Items.Add(container);

                }
                return listBox;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
                return null;
            }
        }

        private static void InventorySelect(object sender, MouseButtonEventArgs e)
        {

            // Example logic for handling the equipment selection
            if (sender is StackPanel stackPanel)
            {
                if (stackPanel != null)
                {
                    object[] itemData = stackPanel.Tag.ToString().Split(',');
                    if (SelectAction == "Equipment")
                    {
                        // Retrieve the associated equipment data from the border's DataContext or Tag
                        EquipEquipment(Convert.ToInt32(itemData[0]));
                    }
                    else if (SelectAction == "ItemShop")
                    {
                        views.ItemShop.SellItemName.Text = ((TextBlock)stackPanel.Children.OfType<Viewbox>().First().Child).Text;
                        views.ItemShop.SellItemImage.Source = stackPanel.Children.OfType<Image>().First().Source;
                        views.ItemShop.SellItemImage.Tag = (string)itemData[0];
                        views.ItemShop.SellItemName.Tag = (string)itemData[1];

                        ItemsOwned = ItemShopHandling.GetAmountOwned((string)itemData[0], (string)itemData[1]);
                        VarAmountInputHandler.AmountOwned = ItemShopHandling.GetAmountOwned((string)itemData[0], (string)itemData[1]);

                        VarAmountInputHandler.Worth = (Convert.ToInt32(itemData[2]) * 0.8).ToString();

                        views.ItemShop.SellItemGrid.Visibility = Visibility.Collapsed;
                        views.ItemShop.ChangeGoldGain();

                    }
                    else if (SelectAction == "UserShop")
                    {
                        views.UserShop.SellItemName.Text = ((TextBlock)stackPanel.Children.OfType<Viewbox>().First().Child).Text;
                        views.UserShop.SellItemImage.Source = stackPanel.Children.OfType<Image>().First().Source;
                        views.UserShop.SellItemImage.Tag = (string)itemData[0];
                        views.UserShop.SellItemName.Tag = (string)itemData[1];

                        ItemsOwned = UserShopHandling.GetAmountOwned((string)itemData[0], (string)itemData[1]);
                        VarAmountInputHandler.AmountOwned = UserShopHandling.GetAmountOwned((string)itemData[0], (string)itemData[1]);

                        VarAmountInputHandler.Worth = (string)itemData[2];

                        views.UserShop.SellItemGrid.Visibility = Visibility.Collapsed;
                        views.UserShop.ChangeGoldGain();
                    }
                    else if (SelectAction == "Inventory")
                    {

                    }
                }

            }
        }
        public static void EquipEquipment(int equipmentId)
        {
            try
            {
                using var context = new GameContext();

                var userEquipment = context.UserEquipments
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .Where(ui => ui.Itemslot == ItemSlot)
                    .FirstOrDefault();
                var equipment = context.Equipment.Where(eq => eq.EquipmentId == equipmentId).First();
                if (userEquipment == null)
                {
                    // Insert new entry using raw SQL
                    context.Database.ExecuteSqlRaw(
                        "INSERT INTO user_equipment (user_id, equipment_id, itemslot) VALUES ({0}, {1}, {2})",
                        MainWindow.UserId, equipmentId, ItemSlot);
                }
                else if (userEquipment.EquipmentId == equipmentId)
                {
                    // Remove existing record
                    context.Database.ExecuteSqlRaw(
                        "DELETE FROM user_equipment WHERE user_id = {0} AND itemslot = {1}",
                        MainWindow.UserId, ItemSlot);
                    equipment.Skin = "";
                }
                else
                {
                    // Update existing record
                    context.Database.ExecuteSqlRaw(
                        "UPDATE user_equipment SET equipment_id = {0} WHERE user_id = {1} AND itemslot = {2}",
                         equipmentId, MainWindow.UserId, ItemSlot);
                }

                EquipmentSlotMap[ItemSlot].Item1.Source = new BitmapImage(new Uri("\\..\\img\\skins\\Equipment_Skins\\" + equipment.Skin + ".png", UriKind.Relative));
                EquipmentSlotMap[ItemSlot].Item2.Text = equipment.Name;

                views.Equipment.EquipmentBorder.Visibility = Visibility.Collapsed;
                views.Equipment.EquipmentList.Items.Clear();
                EquipmentStatBoost();
                UpdateBoostText();

            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        public static void LoadEquipment()
        {
            try
            {
                using var context = new GameContext();

                var userEquipment = context.UserEquipments
                       .Where(ui => ui.UserId == MainWindow.UserId)
                       .ToList();

                foreach (var item in userEquipment)
                {
                    var equipment = context.Equipment.Where(eq => eq.EquipmentId == item.EquipmentId).First();
                    if (item.Itemslot != 0)
                    {
                        EquipmentSlotMap[item.Itemslot].Item1.Source = new BitmapImage(new Uri("\\..\\img\\skins\\Equipment_Skins\\" + equipment.Skin + ".png", UriKind.Relative));
                        EquipmentSlotMap[item.Itemslot].Item2.Text = equipment.Name;
                    }
                }
                EquipmentStatBoost();
                UpdateBoostText();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }

        }
        public static void EquipmentStatBoost()
        {
            try
            {
                RemoveTempBoost();
                using var context = new GameContext();

                // Retrieve all currently equipped items for the user
                var userEquipments = context.UserEquipments
                    .Where(ui => ui.UserId == MainWindow.UserId)
                    .ToList();

                // Reset temporary boosts
                int totalDamageBoost = 0;
                int totalSpeedBoost = 0;
                int totalAttackSpeedBoost = 0;
                int totalHealthBoost = 0;

                // Calculate total boosts from all equipped items
                foreach (var userEquipment in userEquipments)
                {
                    var equipment = context.Equipment
                        .FirstOrDefault(eq => eq.EquipmentId == userEquipment.EquipmentId);

                    if (equipment != null)
                    {
                        totalDamageBoost += equipment.Damage;
                        totalSpeedBoost += equipment.Speed;
                        totalAttackSpeedBoost -= equipment.AttackSpeed;
                        totalHealthBoost += equipment.Health;
                    }
                }

                // Apply the total boosts to the player's stats temporarily

                _UserModel.UserStat.BaseDamage += totalDamageBoost;
                _UserModel.UserStat.BaseSpeed += totalSpeedBoost;
                _UserModel.UserStat.BaseAttackSpeed += totalAttackSpeedBoost;
                _UserModel.UserStat.Health += totalHealthBoost;

                PreviousBoosts = new Tuple<int, int, int, int>(
                    totalDamageBoost,
                    totalSpeedBoost,
                    totalAttackSpeedBoost,
                    totalHealthBoost
                );
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        private static void UpdateBoostText()
        {
            try
            {
                using var context = new GameContext();

                var userStats = context.UserStats.FirstOrDefault(us => us.UserId == MainWindow.UserId);
                if (userStats != null)
                {
                    UpdateExtraStatText(_UserModel.UserStat.Health - userStats.Health, views.Equipment.HealthExtra);
                    UpdateExtraStatText(_UserModel.UserStat.BaseDamage - userStats.BaseDamage, views.Equipment.DamageExtra);
                    UpdateExtraStatText(_UserModel.UserStat.BaseSpeed - userStats.BaseSpeed, views.Equipment.MSExtra);
                    UpdateExtraStatText(_UserModel.UserStat.BaseAttackSpeed - userStats.BaseAttackSpeed, views.Equipment.ASExtra, true);
                }

                views.Equipment.playerASHandler = new();
                views.Equipment.playerDamageHandler = new();
                views.Equipment.playerHPHandler = new();
                views.Equipment.playerMSHandler = new();

                views.Equipment.SetDataContext();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }

        }
        private static void UpdateExtraStatText(int extraStat, OutlinedTextControl textControl, bool isNegative = false)
        {
            if (extraStat != 0)
            {
                textControl.Text = (isNegative ? "- " : "+ ") + Math.Abs(extraStat).ToString();
            }
            else
            {
                textControl.Text = "";
            }
        }
        private static void RemoveTempBoost()
        {
            if (PreviousBoosts != null)
            {
                _UserModel.UserStat.BaseDamage -= PreviousBoosts.Item1;
                _UserModel.UserStat.BaseSpeed -= PreviousBoosts.Item2;
                _UserModel.UserStat.BaseAttackSpeed -= PreviousBoosts.Item3;
                _UserModel.UserStat.Health -= PreviousBoosts.Item4;
                PreviousBoosts = null;
            }
        }
        internal static void InitializeDictionary()
        {
            ran = false;
            EquipmentSlotMap = new()
                  {
           { 1, new Tuple<Image, OutlinedTextControl>(views.Equipment.EquipmetSlot1, views.Equipment.NameSlot1) },
           { 2, new Tuple<Image, OutlinedTextControl>(views.Equipment.EquipmetSlot2, views.Equipment.NameSlot2) },
           { 3, new Tuple<Image, OutlinedTextControl>(views.Equipment.EquipmetSlot3, views.Equipment.NameSlot3) },
           { 4, new Tuple<Image, OutlinedTextControl>(views.Equipment.EquipmetSlot4, views.Equipment.NameSlot4) },
                };
        }
        /*
         *                  SHOP HANDLING
         */
        internal static void BuyItem(int id, int amount, int goldLeft, int isEquipment)
        {
            try
            {
                using var context = new GameContext();

                var itemInventory = context.UserItemInventories
                     .FirstOrDefault();

                if (isEquipment == 1)
                {
                    var equipmentInventory = context.UserEquipmentInventories
                         .Where(us => us.UserId == MainWindow.UserId)
                         .Where(us => us.EquipmentId == id)
                         .FirstOrDefault();

                    AddEquipmentToInventory(id, amount);
                }
                else
                {
                    var items = context.Items.Where(us => us.ItemId == id)
                        .FirstOrDefault();

                    itemInventory = context.UserItemInventories
                        .Where(us => us.UserId == MainWindow.UserId)
                        .Where(us => us.ItemId == id)
                        .FirstOrDefault();

                    if (items.RequiredLevel <= _UserModel.UserStat.Level)
                    {
                        AddItemsToInventory(id, amount);
                    }
                    else
                    {
                        MessageBox.Show("You do not have the required level to purchase this item.");
                        return;
                    }
                }

                // Update existing record
                context.Database.ExecuteSqlRaw(
                    "UPDATE user_item_inventory SET amount = {0} WHERE user_id = {1} AND item_id = {2}",
                   goldLeft, MainWindow.UserId, 1);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        internal static void SellItem(int id, int amount, int goldGain, int isEquipment)
        {
            try
            {

                using var context = new GameContext();

                if (isEquipment == 1)
                {
                    var equipmentInventory = context.UserEquipmentInventories
                         .Where(us => us.UserId == MainWindow.UserId)
                         .Where(us => us.EquipmentId == id)
                         .FirstOrDefault();
                    if (equipmentInventory == null) return;
                    RemoveEquipmentFromInventory(id, amount);
                }
                else
                {
                    var items = context.Items.Where(us => us.ItemId == id)
                        .FirstOrDefault();

                    var itemInventory = context.UserItemInventories
                        .Where(us => us.UserId == MainWindow.UserId)
                        .Where(us => us.ItemId == id)
                        .FirstOrDefault();
                    if (itemInventory == null) return;
                    if (items.RequiredLevel <= _UserModel.UserStat.Level)
                    {
                        RemoveItemsFromInventory(id, amount);
                    }
                    else
                    {
                        MessageBox.Show("You do not have the required level to purchase this item.");
                    }
                }
                ItemsOwned = "0";

                // Update existing record
                context.Database.ExecuteSqlRaw(
                    "UPDATE user_item_inventory SET amount = {0} WHERE user_id = {1} AND item_id = {2}",
                  goldGain, MainWindow.UserId, 1);

            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        internal static void UserBuyItem(int id, int amount, int goldLeft, int isEquipment, string userName)
        {
            try
            {
                using var context = new GameContext();
                var userId = MainWindow.UserId;
                var userLevel = _UserModel.UserStat.Level;

                if (isEquipment == 1)
                {
                    var equipmentInventory = context.UserEquipmentInventories
                        .FirstOrDefault(us => us.UserId == userId && us.EquipmentId == id);

                    if (equipmentInventory != null)
                    {
                        AddEquipmentToInventory(id, amount);
                    }
                }
                else
                {
                    var item = context.Items.FirstOrDefault(us => us.ItemId == id);
                    var itemInventory = context.UserItemInventories
                        .FirstOrDefault(us => us.UserId == userId && us.ItemId == id);

                    if (item != null && item.RequiredLevel <= userLevel)
                    {
                        AddItemsToInventory(id, amount);
                    }
                    else
                    {
                        MessageBox.Show("You do not have the required level to purchase this item.");
                        return;
                    }
                }

                int sellerId = context.Users.First(us => us.Username == userName).UserId;
                var userShopItem = context.Usershops.FirstOrDefault(us => us.UserId == sellerId && (isEquipment == 1 ? us.EquipmentId == id : us.ItemId == id));

                if (userShopItem != null)
                {
                    if (amount >= userShopItem.Amount)
                    {
                        context.Usershops.Remove(userShopItem);
                    }
                    else
                    {
                        userShopItem.Amount -= amount;
                        context.Usershops.Update(userShopItem);
                    }
                }

                var buyerGoldInventory = context.UserItemInventories.First(ui => ui.UserId == userId && ui.ItemId == 1);
                buyerGoldInventory.Amount = goldLeft;
                context.UserItemInventories.Update(buyerGoldInventory);

                var sellerGoldInventory = context.UserItemInventories.First(ui => ui.UserId == sellerId && ui.ItemId == 1);
                sellerGoldInventory.Amount += Convert.ToInt32(VarAmountInputHandler.Cost) * amount;
                context.UserItemInventories.Update(sellerGoldInventory);

                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }

        internal static void UserSellItem(int id, int amount, int goldGain, int isEquipment, string userName)
        {
            try
            {
                using var context = new GameContext();
                var userId = MainWindow.UserId;
                var userLevel = _UserModel.UserStat.Level;

                if (isEquipment == 1)
                {
                    var equipmentInventory = context.UserEquipmentInventories
                        .FirstOrDefault(us => us.UserId == userId && us.EquipmentId == id);

                    if (equipmentInventory == null) return;
                    RemoveEquipmentFromInventory(id, amount);
                }
                else
                {
                    var item = context.Items.FirstOrDefault(us => us.ItemId == id);
                    var itemInventory = context.UserItemInventories
                        .FirstOrDefault(us => us.UserId == userId && us.ItemId == id);

                    if (itemInventory == null) return;

                    if (item != null && item.RequiredLevel <= userLevel)
                    {
                        RemoveItemsFromInventory(id, amount);
                    }
                    else
                    {
                        MessageBox.Show("You do not have the required level to sell this item.");
                        return;
                    }
                }

                var userShopItem = context.Usershops.FirstOrDefault(us => us.UserId == userId && (isEquipment == 1 ? us.EquipmentId == id : us.ItemId == id));

                if (userShopItem != null)
                {
                    userShopItem.Amount += amount;
                    context.Usershops.Update(userShopItem);
                }
                else
                {
                    context.Usershops.Add(new Usershop
                    {
                        UserId = userId,
                        ItemId = isEquipment == 1 ? 1 : id,
                        EquipmentId = isEquipment == 1 ? id : 1,
                        Isequipment = (sbyte)isEquipment,
                        Amount = amount
                    });
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} {e.Source} {e.TargetSite}");
            }
        }
        /*
       *                  AUTHENTICATION HANDLING
       */
        public static bool RegisterUser(string username, string email, string password)
        {
            using var context = new GameContext();
            if (UserExists(username, email, context))
            {
                return false;
            }

            // Hash the password using SHA-256
            var hashedPassword = Hash(password.Trim());

            var user = new Models.User
            {
                Username = username,
                Email = email,
                Password = hashedPassword // Store the hashed password
            };
            context.Users.Add(user);
            context.SaveChanges(); // Save to generate UserId

            var userStats = new Models.UserStat
            {
                UserId = user.UserId,
                Level = 1,
                LevelPoints = 0,
                LevelProgression = 0,
                BaseDamage = 20,
                BaseSpeed = 25,
                BaseAttackSpeed = 1000,
                Health = 100
            };
            var userGameStats = new Models.UserGameStat
            {
                UserId = user.UserId,
                Kills = 0,
                Deaths = 0,
                DamageDone = 0,
                MissedShots = 0,
                HitShots = 0,
                AverageAccuracy = 0,
                WavePr = 0
            };

            var userSkinInventoryList = new List<Models.UserSkinsInventory>
                {
                new Models.UserSkinsInventory
                     {
                    UserId = user.UserId,
        SkinId = 1,
        Purchased = 1
    }
            };
            for (int skinId = 3; skinId <= 16; skinId++)
            {
                userSkinInventoryList.Add(new Models.UserSkinsInventory
                {
                    UserId = user.UserId,
                    SkinId = skinId,
                    Purchased = (sbyte)(skinId == 16 ? 1 : 0)
                });
                if (skinId == 3) skinId = 8;
            }

            var userSkin = new Models.UserSkin
            {
                UserId = user.UserId,
                SkinId = 1
            };

            context.UserStats.Add(userStats);
            context.UserGameStats.Add(userGameStats);
            context.UserSkinsInventories.AddRange(userSkinInventoryList);
            context.UserSkins.Add(userSkin);

            context.SaveChanges();
            // Securely retrieve the UserId after saving changes
            MainWindow.UserId = user.UserId;
            UserModels.Username = user.Username;
            UserModels.Email = user.Email;

            GetStatsFromDB();
            UserKeyBinds.GetDataFromDB();
            EquipmentStatBoost();

            // Create or update the user data file
            CreateOrUpdateUserDataFile(user.UserId, user.Password);

            return true;
        }

        public static bool VerifyUser(string username, string password)
        {
            using var context = new GameContext();

            var user = context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                return false;
            }

            // Hash the entered password
            var enteredPasswordHash = Hash(password.Trim());

            // Check if the hashed password matches the one in the user data file


            return user.Password == enteredPasswordHash;
        }

        private static string Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        private static void CreateOrUpdateUserDataFile(int userId, string encryptedPassword)
        {
            string filePath = "user_data.txt";
            string userData = $"UserId: {userId}, Encrypted Password: {encryptedPassword}";
            File.WriteAllText(filePath, userData + Environment.NewLine);
        }

        internal static bool CheckUserDataFile()
        {
            string filePath = "user_data.txt";
            if (!File.Exists(filePath))
            {
                return false;
            }

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ", " }, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    var idPart = parts[0].Split(new[] { ": " }, StringSplitOptions.None);
                    var passwordPart = parts[1].Split(new[] { ": " }, StringSplitOptions.None);

                    if (idPart.Length == 2 && passwordPart.Length == 2)
                    {
                        if (int.TryParse(idPart[1], out int fileUserId))
                        {
                            string fileHashedPassword = passwordPart[1];

                            using var context = new GameContext();
                            var user = context.Users.SingleOrDefault(u => u.UserId == fileUserId);
                            if (user != null && user.Password == fileHashedPassword)
                            {
                                MainWindow.UserId = user.UserId;
                                UserModels.Email = user.Email;
                                UserModels.Username = user.Username;

                                GetStatsFromDB();
                                UserKeyBinds.GetDataFromDB();
                                EquipmentStatBoost();
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static bool UserExists(string username, string email, GameContext context)
        {
            return context.Users.Any(u => u.Username == username || u.Email == email);
        }
    }
}

