using Space_Shooters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class Variables
    {
        public static int Enemy_ms_Movement = 150; // Default 150
        public static int Enemy_ms_Spawning = 5000; // Default 5000
    }
    internal class PassableVariables
    {
        internal static UserStat userStat = new();
        internal static UserGameStat userGameStat = new();
    }
}
