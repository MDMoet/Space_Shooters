using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Eindopdracht.views;
using static Eindopdracht.classes.UserKeyBinds;
using static Eindopdracht.classes.UserMovement;

namespace Eindopdracht.classes
{
    public class GameKeyDown
    {
        // Create a border object for the pause menu
        internal Border _boPause;
        internal Border _boUser;

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            // Check if the escape key is pressed and the pause menu is visible
            if (e.Key == Pause_Game)
            {
                PausePressed(sender, e);
            }
            if (e.Key == Move_Left)
            {
                MoveLeft(_boUser);
            }
            if (e.Key == Move_Right)
            {
                MoveRight(_boUser);
            }

        }

        // Method to handle the pause button
        public void PausePressed(object sender, KeyEventArgs e)
        {
            // Check if the escape key is pressed
            if (_boPause.Visibility == Visibility.Visible)
            {
                // Hide the pause menu
                ContinueGame(sender, e);
            }
            else
            {
                // Show the pause menu
                PauseGame(sender, e);
            }
        }
        public void ContinueGame(object sender, RoutedEventArgs e)
        {
            // Hide the pause menu
            GameTick.Paused = false;
            _boPause.Visibility = Visibility.Collapsed;
        }
        public void PauseGame(object sender, RoutedEventArgs e)
        {
            // Show the pause menu
            GameTick.Paused = true;
            _boPause.Visibility = Visibility.Visible;
        }
    }
}
