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
        internal bool _waveStarted = true;
        internal Grid _MainGrid = _grMainGrid;
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _waveCleared = false;
        public bool WaveStarted
        {
            get => _waveCleared;
            set
            {
                if (_waveCleared != value)
                {
                    _waveCleared = value;
                    if (_waveCleared)
                    {
                        WaveNumber.IncreaseWave();
                    }
                }
            }
        }

        public void SetWaveCleared(bool value)
        {
            WaveStarted = value;
        }

        internal void StartWave()
        {
            EntityWave.InitiateSpawn initiateSpawn = new EntityWave.InitiateSpawn();
            initiateSpawn.StartSpawning(_MainGrid);
            _waveCleared = false;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class WaveNumber : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static int _wave = 0;
        public int Wave
        {
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
            _wave++;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
