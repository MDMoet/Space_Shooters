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
using System.Windows.Shapes;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>

    public partial class Inventory : UserControl
    {
        internal static ListBox EquipmentList;
        internal static ListBox ItemsList;

        private readonly ViewHandler VarViewHandler;
        public Inventory(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            ItemsList = lbItemInventory;
            EquipmentList = lbEquipmentInventory;
            RetrieveInventoryItems();

            this.VarViewHandler = VarViewHandler;
        }
        public void Return(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToMainMenu();
        }
    }
   
}
