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

namespace Eindopdracht.classes
{
    internal class User
    {
        public static int User_id = 1;
        public static void GetStatsFromDB()
        {
            string connStr = ConfigurationManager.ConnectionStrings["SpaceShooters"].ConnectionString;
            string query = "SELECT wave_pr, deaths, kills, damage_done, missed_shots, hit_shots, average_accuracy, user_stats.level, user_stats.level_progression, user_stats.level_points, user_stats.health, user_stats.base_damage, user_stats.base_attack_speed FROM user_game_stats INNER JOIN user_stats ON user_game_stats.user_id = user_stats.user_id WHERE user_game_stats.user_id = 1;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int[] _userStats = new int[reader.FieldCount];
                        string[] _columnNames = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            _userStats[i] = reader.GetInt32(i);
                            _columnNames[i] = reader.GetName(i);
                        }
                        int k = 0;
                        foreach (string _columnName in _columnNames)
                        {
                            Type statsType = typeof(UserStats).GetField(_columnName) != null ? typeof(UserStats) : typeof(UserGameStats);
                            FieldInfo _field = statsType.GetField(_columnName, BindingFlags.Public | BindingFlags.Static);
                            if (_field != null && _field.FieldType == typeof(int))
                            {
                                int num = _userStats[k];
                                k++;
                                _field.SetValue(null, num);
                            }
                        }                  
                    }
                }
            }
        }
    }
    internal class TempUserStats
    {
        public static int level = UserStats.level;
    }
    internal class UserStats 
    {
        public static int level = 0;
        public static int level_progression = 0;
        public static int level_points = 0;
        public static int health = 0;
        public static int base_damage = 0;
        public static int base_attack_speed = 0;
    }
    internal class UserGameStats
    {
        public static int wave_pr = 0;
        public static int deaths = 0;
        public static int kills = 0;
        public static int damage_done = 0;
        public static int missed_shots = 0;
        public static int hit_shots = 0;
        public static int average_accuracy = 0;
    }
}
