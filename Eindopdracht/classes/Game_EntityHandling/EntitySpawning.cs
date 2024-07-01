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
using static Eindopdracht.classes.GameTick;
using static Eindopdracht.classes.WaveNumber;
using static Eindopdracht.classes.Variables;
using static Eindopdracht.classes.WaveController;
using static Eindopdracht.classes.EntityCreation;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Runtime.InteropServices.WindowsRuntime;
using Eindopdracht.views;

namespace Eindopdracht.classes
{
    internal class EntityWave
    {
        DetermineEntity determineEntity = new DetermineEntity();
        private int wave = _wave;
        private Grid _MainGrid;

        public EntityWave()
        { 
          
        }

        internal int GetEntityWaveAmount()
        {
            int waveAmount = 0;
            string connStr = ConfigurationManager.ConnectionStrings["SpaceShooters"].ConnectionString;
            string query = "SELECT entity_amount FROM wave_information WHERE wave_number = @wave;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@wave", wave);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        waveAmount = reader.GetInt32(0);
                    }
                }
            }
            return waveAmount;
        }
        public class InitiateSpawn
        {
            public InitiateSpawn() {
               
            }
            public async void StartSpawning(Grid _MainGrid)
            { 
                EntityWave entityWave = new EntityWave();
                int entityWaveAmount = entityWave.GetEntityWaveAmount();
                for (int i = 0; i < entityWaveAmount; i++)
                {
                    if (Paused)
                    {
                        await Task.Delay(200);
                        i--; // Decrement i to retry spawning this entity.
                        continue;
                    }
                    DetermineEntity determineEntity = new DetermineEntity();
                    determineEntity.DetermineEntityType(_MainGrid);
                    SpawnEntity(i, _MainGrid);
                    await Task.Delay(Enemy_ms_Spawning); // Wait for the specified tick rate in milliseconds.
                }
            }

            private void SpawnEntity(int index, Grid _MainGrid)
            {
                EntitySpawning.SpawnEntityConfiguration(_MainGrid, _Enemy, index);
            }
        }
    }
    internal static class EntitySpawning
    {
        public static void SpawnEntityConfiguration(Grid _MainGrid, Enemy enemy, int index)
        {
            int sizeIncrease = 5 * enemy.Level;
            Random random = new Random();
            int ranNum = random.Next(-728, 718);

            Border Entity_HitBox = new Border
            {
                BorderBrush = System.Windows.Media.Brushes.Red,
                BorderThickness = new Thickness(1),
                Width = 30 + sizeIncrease,
                Height = 30 + sizeIncrease,
                Margin = new Thickness(ranNum, 0, 0, 430)
            };

            Image Entity = new Image
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/img/Skins/Entity_Skins/{enemy.Skin}.png", UriKind.Absolute)),
                Width = Entity_HitBox.Width - 4,
                Height = Entity_HitBox.Height - 4
            };

            Entity_HitBox.Child = Entity;
            _MainGrid.Children.Add(Entity_HitBox);

            MoveEntity(Entity_HitBox, _MainGrid, enemy, index);
        }

        public static async void MoveEntity(Border Entity, Grid _MainGrid, Enemy enemy, int index)
        {
            while (Entity.Margin.Bottom > -650)
            {
                if (Paused)
                {
                    await Task.Delay(200);
                    continue;
                }
                Entity.Margin = new Thickness(Entity.Margin.Left, Entity.Margin.Top, Entity.Margin.Right, Entity.Margin.Bottom - enemy.Base_speed);
                await Task.Delay(Enemy_ms_Movement);
            }
            EntityWave entityWave = new EntityWave();
            if (_MainGrid.Children.Contains(Entity))
            {
                _MainGrid.Children.Remove(Entity);
                if (index == 0)
                {
                    var waveController = new Eindopdracht.classes.WaveController();
                    // Set _waveCleared to true to indicate the wave is complete
                    waveController.SetWaveCleared(true);

                    Game.SetCenterText($"Wave Cleared!");
                    await Task.Delay(3000);
                    Game.SetCenterText($"Wave: {_wave}");
                    await Task.Delay(3000);
                    Game.InvisCenterText();
                    await Task.Delay(5000);
                    waveController.StartWave();
                }
            }
          
        }
    }
}
