using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_EntityHandling.EntityCreation;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using static Space_Shooters.classes.Game.Game_EntityHandling.DetermineEntity;
using static Space_Shooters.classes.Game.Game_PlayerHandling.PlayerHPHandler;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Mozilla;
using Space_Shooters.views;
using Space_Shooters.Models;
using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace Space_Shooters.classes.Game.Game_EntityHandling
{
    internal class EntityWave
    {
        private readonly int wave = Wave;
        private int waveMax = 5;
        internal static int entityWaveAmount;
        internal static int index;

        internal int GetEntityWaveAmount()
        {
            Random random = new();
            waveMax += 3;
            int waveAmount = random.Next(wave, waveMax + wave);

            return waveAmount;
        }
        public class InitiateSpawn
        {
            public InitiateSpawn() {
               
            }
            public async static void StartSpawning(Grid _MainGrid)
            { 
                EntityWave entityWave = new();
                entityWaveAmount = entityWave.GetEntityWaveAmount();
                MessageBox.Show($"Wave: {Wave} - Amount: {entityWaveAmount}");
                for (index = 0; index < entityWaveAmount; index++)
                {
                    if (Paused)
                    {
                        await Task.Delay(200);
                        index--; // Decrement i to retry spawning this entity.
                        continue;
                    }
                    DetermineEntityType();
                    SpawnEntity(_MainGrid, entityWaveAmount);
                    await Task.Delay(Enemy_ms_Spawning); // Wait for the specified tick rate in milliseconds.
                }
            }

            private static void SpawnEntity(Grid _MainGrid, int entityWaveAmount)
            {
                EntitySpawning.SpawnEntityConfiguration(_MainGrid, _Enemy, entityWaveAmount);
            }
        }
    }
    internal static class EntitySpawning
    {
        public static List<Grid> entities = [];
        public static void SpawnEntityConfiguration(Grid _MainGrid, Enemy enemy, int entityWaveAmount)
        {
            int sizeIncrease = 5 * enemy.Level;
            int lifeIncrease = 7 * enemy.Level;
            if (enemy.Level < 5)
            {
                lifeIncrease -= 7 * enemy.Level;
            }

            Random random = new();
            int ranNum = random.Next(-728, 718);

            Grid Entity_Container = new()
            {
                Width = 31 + sizeIncrease,
                Height = 31 + sizeIncrease,
                Background = System.Windows.Media.Brushes.Transparent,
                Margin = new Thickness(ranNum, 0, 0, 430)
            };

            Border Entity_HitBox = new()
            {
                BorderBrush = System.Windows.Media.Brushes.Red,
                BorderThickness = new Thickness(1),
                Width = 30 + sizeIncrease,
                Height = 30 + sizeIncrease,
                Margin = new Thickness(ranNum, 0, 0, 430)
            };

            Image Entity = new()
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/img/skins/entity_skins/{enemy.Skin}.png", UriKind.Absolute)),
                Width = Entity_HitBox.Width - 4,
                Height = Entity_HitBox.Height - 4
            };
      

            ProgressBar Entity_HealthBar = new()
            {
                Width = Entity_HitBox.Width,
                Height = 5,
                Maximum = enemy.Health + lifeIncrease, 
                Value = enemy.Health + lifeIncrease,
                Background = System.Windows.Media.Brushes.Red,
                Foreground = System.Windows.Media.Brushes.Green,
                BorderBrush = System.Windows.Media.Brushes.Black,
                Margin = new Thickness(0, 0, 0, -29 - sizeIncrease)
            };

            Entity_Container.Children.Add(Entity);
            Entity_Container.Children.Add(Entity_HitBox);
            Entity_Container.Children.Add(Entity_HealthBar);
            _MainGrid.Children.Add(Entity_Container);

            entities.Add(Entity_Container);
            enemies[enemy.Name] = enemy;

            MoveEntity(Entity_Container, _MainGrid, enemy, sizeIncrease, entityWaveAmount);
        }

        public static async void MoveEntity(Grid Entity_Container, Grid _MainGrid, Enemy enemy, int addedWeight, int entityWaveAmount)
        {
            while (Entity_Container.Margin.Bottom > -650)
            {
                if (Paused)
                {
                    await Task.Delay(200);
                    continue;
                }
                Entity_Container.Margin = new Thickness(Entity_Container.Margin.Left, Entity_Container.Margin.Top, Entity_Container.Margin.Right, Entity_Container.Margin.Bottom - enemy.Base_speed);
                await Task.Delay(Enemy_ms_Movement + addedWeight);

            }

            if (_MainGrid.Children.Contains(Entity_Container))
            {
                _MainGrid.Children.Remove(Entity_Container);
                entities.Remove(Entity_Container);
                WaveCheck.CheckForEntities(_MainGrid, Entity_Container, entityWaveAmount);
                DecreaseHP(enemy.Base_damage);
            }
        }
    }
}
