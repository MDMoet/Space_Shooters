using Space_Shooters.classes;
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
using static Space_Shooters.classes.UserKeyBinds;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using static Space_Shooters.classes.Game.Game_PlayerHandling.User;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using Space_Shooters.classes.Game.Game_EntityHandling;
using Space_Shooters.classes.Game.Game_CollisionHandling;
using Space_Shooters.classes.Game.Game_PlayerHandling;
using Space_Shooters.classes.Game.Game_VariableHandling;
using Space_Shooters.classes.Game.Game_UIHandling;
using Space_Shooters.Models;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
        private static OutlinedTextControl CenterBlock = new();
        private readonly ViewHandler VarViewHandler;

        public Game(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            // Set the view in the ViewHandler to this view
            this.VarViewHandler = VarViewHandler;
            // Create a new instance of the StartingTimer class
            StartingTimer timer = new();
            // Subscribe to the CountdownCompleted event
            timer.CountdownCompleted += Timer_CountdownCompleted;
            // Set the datacontext of the textblock to the timer (Time in StartingTimer class)
            tbCenterText.DataContext = timer;
            // Set the CenterBlock to the OutlinedTextControl
            CenterBlock = tbCenterText;
            // Create a new instance of the GameViewModel class
            GameViewModel gameViewModel = new();
            // Set the datacontext of the textblock to the waveCounter (Wave in WaveNumber class)
            tbWaveNum.DataContext = gameViewModel;
            // Create a new instance of the CollisionLoop class
            CollisionLoop collisionLoop = new(MainGrid);
            // Start the collision loop
            collisionLoop.Start();

            Paused = false;
            Tick();
            GetDataFromDB();
            GetStatsFromDB(1);

            // Set the datacontext of the progressbar to the hpAmount (PlayerHP in PlayerHPHandler class)
            PlayerHPHandler playerHPHandler = new();
            pbUserHealth.DataContext = playerHPHandler;
            pbUserHealth.Maximum = PlayerHPHandler.PlayerHP;
        }

        private async void Timer_CountdownCompleted(object? sender, EventArgs e)
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
                BindingOperations.ClearBinding(tbCenterText, OutlinedTextControl.TextProperty);
                // Set the text to "Start!"
                tbCenterText.Text = "Start!";
                await Wait.WaitInSeconds(1); // Wait for 1 second
                // Remove the textblock from the grid
                tbCenterText.Visibility = Visibility.Collapsed;
                WaveNumberHandling waveController = new ();
                waveController.StartWave();
                break;
            }
        }

        public static void SetCenterText(object text)
        {
            if (CenterBlock != null)
            {
                CenterBlock.Visibility = Visibility.Visible;
                CenterBlock.Text = text?.ToString() ?? string.Empty;
            }
        }
        public static void InvisCenterText()
        {
            CenterBlock.Visibility = Visibility.Collapsed;
        }
        private void ExitToDesktop(object sender, RoutedEventArgs e)
        {
            // Close the application
            System.Windows.Application.Current.Shutdown();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the window
            var window = Window.GetWindow(this);
            // Subscribe to the KeyDown event
            GameKeyDown._grMainGrid = MainGrid;

            GameKeyDown keyDown = new()
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
