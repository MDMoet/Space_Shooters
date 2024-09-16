using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class RNGRandomiser
    {
        private static Random _random = new();
        internal static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
