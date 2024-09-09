using Space_Shooters.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Space_Shooters.classes.Game.Game_EntityHandling.WaveNumber;
using static Space_Shooters.classes.Game.Game_EntityHandling.EntitySpawning;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using Space_Shooters.classes.Game.Game_VariableHandling;


namespace Space_Shooters.classes.Game.Game_EntityHandling
{
    internal class WaveClearHandling
    {

        public WaveClearHandling() { }

        internal static async void WaveCleared()
        {
            while (true)
            {
                if (Paused)
                {
                    // Wait for 200ms to reduce the CPU usage
                    await Task.Delay(200);
                    // Skip the rest of the loop but don't break it
                    continue;
                }
                // Set the center text to "Wave Cleared!"
                views.Game.SetCenterText($"Wave Cleared!");
                // Increase the wave number
                IncreaseWave();
                // Wait for 2 seconds
                await Wait.WaitInSeconds(2);
                // set the center text to the wave number
                views.Game.SetCenterText($"Wave: {Wave}");
                // Wait for 2 seconds
                await Wait.WaitInSeconds(2);
                // Clear the center text
                views.Game.InvisCenterText();
                // Wait for 1 second
                await Wait.WaitInSeconds(1);
                WaveNumberHandling waveController = new();
                waveController.StartWave();
                break;
            }
        }
    }
    internal class WaveCheck
    {
        public WaveCheck()
        {

        }
        internal static void CheckForEntities(Grid _MainGrid, Grid Entity_Container, int _WaveNum)
        {
            bool ContainsEntity = entities.Count == 0;
            if (EntityWave.index != _WaveNum && !ContainsEntity)
            {
                return;
            }
            else if (EntityWave.index == _WaveNum && ContainsEntity)
            {
                WaveClearHandling.WaveCleared();
            }
        }
    }
}
