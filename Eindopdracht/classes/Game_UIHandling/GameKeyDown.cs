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
using static Eindopdracht.classes.UserStats;
using System.Runtime.CompilerServices;


namespace Eindopdracht.classes
{
    public class GameKeyDown
    {
        // Create a border object for the pause menu
        internal Border _boPause;
        internal Border _boUser;
        internal static Grid _grMainGrid;
        internal bool wait = false;
        private readonly UserActions userActions = new UserActions();

        public async void KeyPressed(object sender, KeyEventArgs e)
        {
            // Check if the escape key is pressed and the pause menu is visible
            if (e.Key == Pause_Game)
            {
                PausePressed(sender, e);
            }
            // Check if the game is paused
            if (GameTick.Paused == true)
            {
                // Skip the rest of the method if game is paused
                return;
            }
            // Check if the movement keys are pressed
            if (e.Key == Move_Left)
            {
                // Move the user to the left
                userActions.MoveLeft(_boUser);
            }
            if (e.Key == Move_Right)
            {
                // Move the user to the right
                userActions.MoveRight(_boUser);
            }
            // Check if the attack key is pressed
            if (e.Key == Attack_1)
            {
                if (!wait)
                {
                    userActions.Attack1(_boUser, _grMainGrid);
                    wait = true;
                    await Task.Delay(base_attack_speed);
                    wait = false;
                }
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

