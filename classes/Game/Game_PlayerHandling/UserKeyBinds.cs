using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using System.Reflection;
using System.Collections.Generic;
using Space_Shooters.Context;

namespace Space_Shooters.classes
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
            using var context = new GameContext();

            var keyBinds = context.UserKeybinds
                .Where(e => e.UserId == MainWindow.UserId)
                .Select(uk => new
                {
                    KeybindEnum = uk.Keybind.KeybindEnum1,
                    uk.Action.Action
                })
                .ToList();

            var keyBindsDict = new Dictionary<string, Key>();
            foreach (var keyBind in keyBinds)
            {
                if (Enum.TryParse(keyBind.KeybindEnum.ToString(), out Key key))
                {
                    keyBindsDict[keyBind.Action] = key;
                }
            }

            foreach (var keyBind in keyBindsDict)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                FieldInfo field = typeof(UserKeyBinds).GetField(keyBind.Key, BindingFlags.Public | BindingFlags.Static);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                field?.SetValue(null, keyBind.Value);
            }
        }        
    }
}
