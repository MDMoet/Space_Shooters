using Microsoft.EntityFrameworkCore.Query.Internal;
using Space_Shooters.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using Space_Shooters.classes.Game.Game_DataHandling;
using Space_Shooters.classes.Game.Game_EntityHandling;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class Variables
    {
        public static int Enemy_ms_Movement = 150; // Default 150
        public static int Enemy_ms_Spawning = 3000; // Default 5000
        public static int Entity_Wave_Amount = 1; // Default 1

        public static int Bullet_Damage_Multiplier = 10; // Default 1
        private static int Bullet_BaseDamage = _UserModel.UserStat.BaseDamage; // Default 20. Depends on user's level
        public static int Bullet_Damage = Bullet_BaseDamage * Bullet_Damage_Multiplier;  // Actual damage of bullet

       internal static bool KeyDownSubscribed = false; // Default False
        internal static int EntitiesLeft = 0; // Default 0

        internal static List<Grid> EntitiesList = [];
        internal static List<Border> BulletsList = [];
    }
    internal class PassableVariables
    {
        internal static WaveModel _WaveModel;
        internal static WindowModel _WindowModel;
        internal static UserModel _UserModel;
        internal static EntityModel _EntityModel;

        internal static Enemy _Enemy;
    }
}
