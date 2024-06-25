using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using static Eindopdracht.classes.UserActions;
using static Eindopdracht.classes.UserStats;

namespace Eindopdracht.classes
{
    public class GameTick
    {
        // Create an internal bool to hold the value of the game being paused
        internal static bool Paused = false;
        // Create an internal int to store the tickrate
        internal static int _tickRate = 20;
        // Create an internal int to store the ms
        internal static int _ms = 1000;
        // Create an internal int to store the amount of ms that pass each tick
        internal static int tickMs = _ms / _tickRate;
        // Create a private int to store a copy of the ms
        private static int _msTemp = _ms;

        public static int TickRateMs
        {
            // Get the amount of ms that have passed with the tickrate
            get => tickMs;
            set
            {
                // Set the tickrate
                _ms = value;
            }
        }
        public async static void Tick()
        {
            // Decrease the time until it reaches 0
            int _baseSpeed = base_attack_speed;
           while(_msTemp >= 0)
            {
                if (Paused)
                {
                    // Wait for 200ms to reduce the CPU usage
                    await Task.Delay(200);
                    // Skip the rest of the loop
                    continue;
                }
                // Wait for the tickrate
                await Task.Delay(TickRateMs);
                // Decrease the time
                _msTemp -= TickRateMs;
              
                // Check if the time has reached 0
                if (_msTemp == 0)
                {
                    // Reset the time
                    _msTemp = _ms;
                    // Return the ms that have passed
                    PassedTimeSecond();
                }
            }
        }
        public static int PassedTimeSecond()
        {
            // Return the ms that have passed
            return _ms;
        }
    }
}
