using Eindopdracht.classes;
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
using System.Windows.Shapes;

namespace Eindopdracht.views
{
    /// <summary>
    /// Interaction logic for Equipment.xaml
    /// </summary>
    public partial class Equipment : UserControl
    {
        private readonly ViewHandler VarViewHandler;
        public Equipment(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            this.VarViewHandler = VarViewHandler;
        }
        public void Equipmentbt(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToEquipment();
        }
        public void Skins(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToSkins();
        }
        public void Return(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToMainMenu();
        }
    }
}
