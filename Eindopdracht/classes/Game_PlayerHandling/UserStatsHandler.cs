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
using static Eindopdracht.classes.UserGameStats;
using static Eindopdracht.classes.UserStats;

namespace Eindopdracht.classes
{
    internal class GameStatsHandler
    {
        public static void UpdateGameStats()
        {
            // Get the connection string from the App.config file
            string connStr = ConfigurationManager.ConnectionStrings["SpaceShooters"].ConnectionString;

            // Define the SQL query with parameters
            string query = "UPDATE user_game_stats SET " +
                           "wave_pr = @wavePr, deaths = @deaths, kills = @kills, " +
                           "damage_done = @damageDone, missed_shots = @missedShots, " +
                           "hit_shots = @hitShots, average_accuracy = @averageAccuracy " +
                           "WHERE user_id = @userId;";

            // Create a connection to the database
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                // Create a command to execute the query
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Add the parameters and their values
                cmd.Parameters.AddWithValue("@wavePr", wave_pr);
                cmd.Parameters.AddWithValue("@deaths", deaths);
                cmd.Parameters.AddWithValue("@kills", kills);
                cmd.Parameters.AddWithValue("@damageDone", damage_done);
                cmd.Parameters.AddWithValue("@missedShots", missed_shots);
                cmd.Parameters.AddWithValue("@hitShots", hit_shots);
                cmd.Parameters.AddWithValue("@averageAccuracy", average_accuracy);
                cmd.Parameters.AddWithValue("@userId", 1); // Assuming user_id is 1

                // Open the connection
                conn.Open();

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
    internal class UserStatsHandler
    {
        public static void UpdateGameStats()
        {
            // Get the connection string from the App.config file
            string connStr = ConfigurationManager.ConnectionStrings["SpaceShooters"].ConnectionString;

            // Define the SQL query with parameters
            string query = "UPDATE user_stats SET level = @level, level_progression = @levelProgression, level_points = @levelPoints, health = @health, base_damage = @baseDamage, base_attack_speed = @baseAttackSpeed WHERE user_id = @userId;";

            // Create a connection to the database
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                // Create a command to execute the query
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Add the parameters and their values
                cmd.Parameters.AddWithValue("@level", level);
                cmd.Parameters.AddWithValue("@levelProgression", level_progression);
                cmd.Parameters.AddWithValue("@levelPoints", level_points);
                cmd.Parameters.AddWithValue("@health", health);
                cmd.Parameters.AddWithValue("@baseDamage", base_damage);
                cmd.Parameters.AddWithValue("@baseAttackSpeed", base_attack_speed);
                cmd.Parameters.AddWithValue("@userId", 1); // Assuming user_id is 1

                // Open the connection
                conn.Open();

                // Execute the query
                cmd.ExecuteNonQuery();
            }
        }
    }
}
