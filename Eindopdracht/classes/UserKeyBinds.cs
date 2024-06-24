using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using System.Reflection;

namespace Eindopdracht.classes
{
    public class UserKeyBinds
    {
        public static Key Pause_Game;
        public static Key Move_Left;
        public static Key Move_Right;
        public static Key Attack_1;
        public static Key Attack_2;
        public static Key Attack_3;

        public static void GetDataFromDB()
        {
            // Get the connection string from the App.config file
            string connStr = ConfigurationManager.ConnectionStrings["SpaceShooters"].ConnectionString;
            // Query to get the keybinds from the database
            string query = "SELECT keybind_enums.keybind_enum, actions.action FROM user_keybinds INNER JOIN keybind_enums ON user_keybinds.keybind_id = keybind_enums.keybind_id LEFT JOIN default_keybinds  ON user_keybinds.keybind_id = default_keybinds.keybind_id INNER JOIN actions ON user_keybinds.action_id = actions.action_id;";

            // Create a connection to the database
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                // Create a command to execute the query
                MySqlCommand cmd = new MySqlCommand(query, conn);
                // Open the connection
                conn.Open();
                 
                // Execute the query
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    // Read the results
                    while (reader.Read())
                    {
                        // Get the keybind enum and action name from the database
                        string _keybindEnum = reader.GetInt32(0).ToString(); // Assuming that the keybind enum is the first column
                        // Get the action name from the database
                        string _action = reader.GetString(1); // Assuming the action name is the second column

                        // Use reflection to set the static field value
                        FieldInfo _field = typeof(UserKeyBinds).GetField(_action, BindingFlags.Public | BindingFlags.Static);
                        // Check if the field exists and is of type Key
                        if (_field != null && _field.FieldType == typeof(Key))
                        {
                            // Parse the keybindId to a Key enum
                            Key key = (Key)Enum.Parse(typeof(Key), _keybindEnum);
                            // Set the field value to the key
                            _field.SetValue(null, key);
                        }
                    }
                }
            }
        }
    }
}
