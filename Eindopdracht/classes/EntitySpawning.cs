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

namespace Eindopdracht.classes
{
    
    internal class EntityWave
    {
        private int wave = _wave;
        private Grid mainGrid;
        private Enemy enemy;

        public EntityWave(Grid MainGrid, Enemy enemy)
        {
            this.mainGrid = MainGrid;
            this.enemy = enemy;
            StartSpawning();
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

        private async void StartSpawning()
        {
            int entityWaveAmount = GetEntityWaveAmount();
            for (int i = 0; i < entityWaveAmount; i++)
            {
                if (Paused)
                {
                    await Task.Delay(200);
                    i--; // Decrement i to retry spawning this entity.
                    continue;
                }
                SpawnEntity(i);
                await Task.Delay(Enemy_ms_Spawning); // Wait for the specified tick rate in milliseconds.
            }
        }

        private void SpawnEntity(int index)
        {
            EntitySpawning.SpawnEntity(mainGrid, enemy, index);
        }
    }

    internal static class EntitySpawning
    {
        public static void SpawnEntity(Grid MainGrid, Enemy enemy, int index)
        {
            int sizeIncrease = 6 * enemy.Level;
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
            MainGrid.Children.Add(Entity_HitBox);

            MoveEntity(Entity_HitBox, MainGrid, enemy, index);
        }

        public static async void MoveEntity(Border Entity, Grid MainGrid, Enemy enemy, int index)
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
            EntityWave entityWave = new EntityWave(MainGrid, enemy);
            if (MainGrid.Children.Contains(Entity))
            {
                MainGrid.Children.Remove(Entity);
            } else if (index > entityWave.GetEntityWaveAmount())
            {
                _waveCleared = true;
            }
        }
    }
}
