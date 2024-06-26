﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Eindopdracht.classes.GameTick;


namespace Eindopdracht.classes
{
    public class StartingTimer : INotifyPropertyChanged
    {
        // Event that clients can subscribe to, to be notified when a property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Event that clients can subscribe to, to be notified when the countdown reaches 0
        public event EventHandler CountdownCompleted;

        // Create a private int to store the time
        private int _time = 1;
        public int Time
        {
            // Get the time
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged();
                    // Check if the countdown has reached 0 and raise the event
                    if (_time == 0)
                    {
                        OnCountdownCompleted();
                    }
                }
            }
        }

        public StartingTimer()
        {
            // Start the countdown
            StartCountdown();
        }

        private async void StartCountdown()
        {
            // Wait for 1 second and decrease the timer until it reaches 0
            while (Time > 0)
            {   
                // Loops through this for every tick, making you wait 1 second
                for (int i = 0; i < _tickRate; i++)
                {
                    // Check if the game is paused
                    if (Paused)
                    {
                        // Wait for 200ms to reduce the CPU usage
                        await Task.Delay(200);
                        // Minus the i to ensure that the loop stays running even while paused
                        i--;
                        // Skip the rest of the loop and wait for the next tick
                        continue;
                    }
                    // Wait for the tick
                    await Task.Delay(tickMs);
                }
                // Wait for 1 second
                Time--; // Decrease the timer
            }
        }
        // Method to raise the PropertyChanged event
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Method to raise the CountdownCompleted event
        protected void OnCountdownCompleted()
        {
            CountdownCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
