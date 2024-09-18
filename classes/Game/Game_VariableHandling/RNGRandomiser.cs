using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mysqlx.Notice.Warning.Types;
using static Space_Shooters.classes.Game.Game_VariableHandling.Variables;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using System.Windows.Forms;

namespace Space_Shooters.classes.Game.Game_VariableHandling
{
    internal class RNGRandomiser
    {
        internal static bool LayeredRandomise(int requiredLevel, int worth)
        {
            // Adjust this factor as needed to fit your game's balance.
            double difficultyFactor = (1 + Difficulty) * 1.3;
            
            // Calculate a base chance based on worth and level.
            double baseChance = (100.0 / worth) * requiredLevel;

            // Apply the difficulty factor to the base chance.
            double finalChance = baseChance  * difficultyFactor;

            // Ensure the final chance does not exceed 100%.
            finalChance = Math.Min(finalChance, 100.0);

            // Generate a random number to determine if the item drops.
            Random random = new Random();
            double roll = random.NextDouble() * 100;

            return roll <= finalChance;
        }
        internal static int GoldDropped(int Level_)
        {
            // Level_ * Difficulty determines the center of the range of gold that can be dropped.
            Level_ = Level_ * Difficulty;

            // The minimum gold dropped is half the level.
            double min_ = Level_ / 2;

            // The maximum gold dropped is 1.5 times the level.
            double max_ = Level_ * (1.5 + (Wave / 10));

            // Generate a random number between the min and max values.
            int _Gold = RandomNumber(Convert.ToInt32(min_), Convert.ToInt32(max_) );

            // Ensure that at least 1 gold is dropped.
            _Gold = (_Gold == 0) ? 1 : _Gold;

            // Return the gold value.
            return _Gold;
        }
        internal static int RandomNumber(int min, int max)
        {
            Random _random = new();
            return _random.Next(min, max);
        }
    }
}
