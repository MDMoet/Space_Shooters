using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using System.Reflection;
using System.Collections.Generic;

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
            // The connection string
            string connStr = ConfigurationManager.ConnectionStrings["SpaceShooters"].ConnectionString;
            // The query
            string query = "SELECT keybind_enums.keybind_enum, actions.action FROM user_keybinds INNER JOIN keybind_enums ON user_keybinds.keybind_id = keybind_enums.keybind_id LEFT JOIN default_keybinds  ON user_keybinds.keybind_id = default_keybinds.keybind_id INNER JOIN actions ON user_keybinds.action_id = actions.action_id;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    var keyBinds = new Dictionary<string, Key>();
                    while (reader.Read())
                    {
                        string _keybindEnum = reader.GetInt32(0).ToString();
                        string _action = reader.GetString(1);

                        if (Enum.TryParse(_keybindEnum, out Key key))
                        {
                            keyBinds[_action] = key;
                        }
                    }

                    foreach (var keyBind in keyBinds)
                    {
                        FieldInfo _field = typeof(UserKeyBinds).GetField(keyBind.Key, BindingFlags.Public | BindingFlags.Static);
                        _field?.SetValue(null, keyBind.Value);
                    }
                }
            }
        }
    }
}
