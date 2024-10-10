using Space_Shooters.classes;
using System;
using System.Collections.Generic;
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
using static Space_Shooters.classes.General.User_DataHandling.PlayerDataHandling;
<<<<<<< HEAD
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
=======
>>>>>>> 0f8688f88e38822a5e631e4183dc057a8afd8f48
using System.Windows.Shapes;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>

    public partial class Inventory : UserControl
    {
<<<<<<< HEAD
=======
        internal static ListBox EquipmentList;
        internal static ListBox ItemsList;
>>>>>>> 0f8688f88e38822a5e631e4183dc057a8afd8f48

        private readonly ViewHandler VarViewHandler;
        public Inventory(ViewHandler VarViewHandler)
        {
            InitializeComponent();
<<<<<<< HEAD
            boItemInventory.Child = RetrieveInventoryItems();
            boEquipmentInventory.Child = RetrieveInventoryEquipment();

            SelectAction = "Inventory";
=======
            ItemsList = lbItemInventory;
            EquipmentList = lbEquipmentInventory;
            RetrieveInventoryItems();
>>>>>>> 0f8688f88e38822a5e631e4183dc057a8afd8f48

            this.VarViewHandler = VarViewHandler;
        }
        public void Return(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            VarViewHandler.Return();
=======
            VarViewHandler.GoToMainMenu();
>>>>>>> 0f8688f88e38822a5e631e4183dc057a8afd8f48
        }
    }
   
}
