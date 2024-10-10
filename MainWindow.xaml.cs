using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using Space_Shooters.classes;
using Space_Shooters.views;
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
<<<<<<< HEAD
using static Space_Shooters.classes.General.User_DataHandling.UserKeyBinds;
=======
>>>>>>> 0f8688f88e38822a5e631e4183dc057a8afd8f48
using Space_Shooters.classes.Game.Game_DataHandling;
using Space_Shooters.classes.Game.Game_EntityHandling;

namespace Space_Shooters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewHandler VarViewHandler { get; set; }
        internal static Border BoDuplicateKeybind;
        internal static Border BoSelectItem;
        internal static Border BoNoInputGiven;
        internal static int UserId;
        public MainWindow()
        {
            InitializeComponent();
            // Get the stats from the database
<<<<<<< HEAD
=======
            GetStatsFromDB();
>>>>>>> 0f8688f88e38822a5e631e4183dc057a8afd8f48
            VarViewHandler = new ViewHandler();
            DataContext = VarViewHandler;

            if (CheckUserDataFile())
            {
                VarViewHandler.GoToMainMenu();
                GetStatsFromDB();
                GetDataFromDB();
              
                EquipmentStatBoost();
            }
            else
            {
                VarViewHandler.GoToRegister();
                return;
            }
            InitializeObjects();

        }
        private void InitializeObjects()
        {
            BoDuplicateKeybind = boDuplicateKeyBind;
            BoSelectItem = boSelectItem;
            BoNoInputGiven = boNoInputGiven;
        }
    }
}

