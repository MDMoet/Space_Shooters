using Eindopdracht.classes;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.CodeDom;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using static Eindopdracht.classes.UserKeyBinds;
using static Eindopdracht.classes.GameStatsHandler;
using static Eindopdracht.classes.GameTick;
using static Eindopdracht.classes.UserStats;
using static Eindopdracht.classes.User;

namespace Eindopdracht.views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
        private readonly ViewHandler VarViewHandler;
        public Game(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            // Set the view in the ViewHandler to this view
            this.VarViewHandler = VarViewHandler;
            // Create a new instance of the StartingTimer class
            StartingTimer timer = new StartingTimer();
            // Subscribe to the CountdownCompleted event
            timer.CountdownCompleted += Timer_CountdownCompleted;
            // Set the datacontext of the textblock to the timer (Time in StartingTimer class)
            tbGameTimer.DataContext = timer;

            // Create a new instance of the WaveNumber class
            WaveNumber waveCounter = new WaveNumber();
            // Set the datacontext of the textblock to the waveCounter (Wave in WaveNumber class)
            tbWaveNum.DataContext = waveCounter;

            // Create a new instance of the WaveNumber class
            PlayerHPHandler hpAmount = new PlayerHPHandler();
            // Set the datacontext of the textblock to the waveCounter (Wave in WaveNumber class)
            pbUserHealth.DataContext = hpAmount;
            pbUserHealth.Maximum = health;

            Paused = false;
            Tick();
            GetDataFromDB();
            GetStatsFromDB();
        }

        private async void Timer_CountdownCompleted(object sender, EventArgs e)
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
                // Clear the binding to avoid an error when setting the text
                BindingOperations.ClearBinding(tbGameTimer, OutlinedTextControl.TextProperty);
                // Set the text to "Start!"
                tbGameTimer.Text = "Start!";
                // Loops through this for every tick, making you wait 1 second, change the '* 1' into any diserable number of seconds you want it to way.
                for (int i = 0; i < _tickRate * 1; i++)
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
                // Remove the textblock from the grid
                MainGrid.Children.Remove(tbGameTimer);
                WaveController waveController = new WaveController();
                waveController.StartWave();
                break;
            }
        }

        private void ExitToDesktop(object sender, RoutedEventArgs e)
        {
            // Close the application
            UpdateGameStats();

            System.Windows.Application.Current.Shutdown();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the window
            var window = Window.GetWindow(this);
            // Subscribe to the KeyDown event
            GameKeyDown._grMainGrid = MainGrid;

            GameKeyDown keyDown = new GameKeyDown
            {
                // Set the pause menu to the border variable in the GameKeyDown class
                _boPause = boPauseGame,
                // Set the user to the border variable in the GameKeyDown class
                _boUser = boUserHitBox
            };
            // Subscribe to the KeyDown event
            window.KeyDown += keyDown.KeyPressed;
        }
        public void ContinueGame(object sender, RoutedEventArgs e)
        {
            // Hide the pause menu
            boPauseGame.Visibility = Visibility.Collapsed;
            Paused = false;
        }
    }
}
