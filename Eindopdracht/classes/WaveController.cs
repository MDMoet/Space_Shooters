using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Eindopdracht.classes.GameTick;

namespace Eindopdracht.classes
{
    internal class WaveController
    {
        internal static bool _gameStarted = false;
    }
    internal class WaveNumber : INotifyPropertyChanged
    {
        // Event that clients can subscribe to, to be notified when a property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Event that clients can subscribe to, to be notified when the countdown reaches 0
        public event EventHandler CountdownCompleted;

        // Create a private int to store the time
        private int _wave = 0;
        public int Wave
        {
            // Get the time
            get => _wave;
            set
            {
                if (_wave != value)
                {
                    _wave = value;
                    OnPropertyChanged();
                }
            }
        }
        public WaveNumber()
        {
            // Start the countdown
            StartWaveCount();
        }

        private void StartWaveCount()
        {
            // Wait for 1 second and decrease the timer until it reaches 0
            Wave++;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
