using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Eindopdracht.classes
{
    public class GameTick
    {
        // Create a public bool to check if the game is paused
        public static bool Paused = false;

        // Create a private int to store the tickrate
        private static int _tickRate = 20;
        // Create a private int to store the ms
        private static int _ms = 1000;
        // Create a private int to store a copy of the ms
        private static int _msTemp = _ms;

        public static int TickRateMs
        {
            // Get the amount of ms that have passed with the tickrate
            get => _ms / _tickRate;
            set
            {
                // Set the tickrate
                _ms = value;
            }
        }
        public async static void Tick()
        {
            // Decrease the time until it reaches 0
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
                    PassedTimeMs();
                }
            }
        }
        
        public static int PassedTimeMs()
        {
            // Return the ms that have passed
            return _ms;
        }
    }
}
