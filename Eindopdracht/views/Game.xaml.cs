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
using static Eindopdracht.classes.GameTick;

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
            // Set the datacontext of the view to the ViewHandler
            StartingTimer timer = new StartingTimer();
            // Subscribe to the CountdownCompleted event
            timer.CountdownCompleted += Timer_CountdownCompleted;
            // Set the datacontext of the textblock to the timer (Time in StartingTimer class)
            tbGameTimer.DataContext = timer;
            // Start the ticks, the parameter is false because the game is not paused
            Paused = false;
            Tick();
            GetDataFromDB();
        }

        private async void Timer_CountdownCompleted(object sender, EventArgs e)
        {
            // Clear the binding to avoid an error when setting the text
            BindingOperations.ClearBinding(tbGameTimer, OutlinedTextControl.TextProperty);
            // Set the text to "Start!"
            tbGameTimer.Text = "Start!";
            // Wait for 750ms
            await Task.Delay(750);
            // Remove the text (RemoveAt(0) is possible, because the textblock will always be the first item in that grid)
            grUserUI.Children.RemoveAt(0);
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
        }

        private void tbGameTimer_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
