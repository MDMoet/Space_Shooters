using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Space_Shooters.views;

namespace Space_Shooters.classes
{
    public class ViewHandler : INotifyPropertyChanged
    {
        // Create a private UserControl to store the current view
        private UserControl _view;

        // Create a public UserControl to store the current view
        public UserControl View
        {
            // Get the current view
            get { return _view; }
            set
            {
                // Set the current view
                _view = value;
                // Call the OnPropertyChanged method
                OnPropertyChanged();
            }
        }

        public ViewHandler()
        {
            _view = new views.Game(this); // Initialize the _view field to avoid CS8618 error
        }

        // Create methods to change the view
        public void GoToMainMenu()
        {
            View = new MainMenu(this);
        }
        public void GoToGameMenu()
        {
            View = new GameMenu(this);
        }
        public void GoToCharacter()
        {
            View = new Character(this);
        }
        public void GoToInventory()
        {
            View = new Inventory();
        }
        public void GoToItemShop()
        {
            View = new ItemShop();
        }
        public void GoToSettings()
        {
            View = new Settings();
        }
        public void GoToEquipment()
        {
            View = new Equipment(this);
        }
        public void GoToSkins()
        {
            View = new Skins(this);
        }
        public void GoToGame()
        {
            View = new views.Game(this);
        }

        // Create an event to notify the view that the view has changed and for methods to subscribe to the event
        public event PropertyChangedEventHandler? PropertyChanged;

        // Create a method to call the event
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
