using Eindopdracht.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Eindopdracht.classes.WaveController;
using static Eindopdracht.classes.GameKeyDown;
using System.Windows.Controls;

namespace Eindopdracht.classes
{
    internal class WaveController : INotifyPropertyChanged
    {
        internal static bool _waveStarted = true;
        internal Grid _MainGrid = _grMainGrid;
        // Event that clients can subscribe to, to be notified when a property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Create a private int to store the time
        internal static bool _waveCleared = false;
        public bool WaveStarted
        {
            // Get the time
            get => _waveCleared;
            set
            {
                if (_waveCleared != value)
                {
                    _waveCleared = value;
                    StartWave();
                }
            }
        }
        internal void StartWave() 
        { 
            DetermineEntity determineEntity = new DetermineEntity();
            determineEntity.DetermineEntityType(_MainGrid);
            _waveCleared = false;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    internal class WaveNumber : INotifyPropertyChanged
    {
        // Event that clients can subscribe to, to be notified when a property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Create a private int to store the time
        internal static int _wave = 0;
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
            IncreaseWave();
        }

        internal static void IncreaseWave()
        {
            if(_waveStarted)
            {
                _wave++;
                _waveCleared = false;
            }
            
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
