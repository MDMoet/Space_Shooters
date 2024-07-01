using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Eindopdracht.classes.UserStats;

namespace Eindopdracht.classes
{
    internal class PlayerHPHandler : INotifyPropertyChanged
    {
        // Event that clients can subscribe to, to be notified when a property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Create a private int to store the time
        internal static int _playerHP = health;
        public int PlayerHP
        {
            // Get the time
            get => _playerHP;
            set
            {
                if (_playerHP != value)
                {
                    _playerHP = value;
                    OnPropertyChanged();
                }
            }
        }

        // Method to notify the UI that a property has changed
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
