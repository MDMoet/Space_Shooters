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
    /// Interaction logic for Character.xaml
    /// </summary>
    public partial class Character : UserControl
    {
        private readonly ViewHandler VarViewHandler;
        public Character(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            this.VarViewHandler = VarViewHandler;
        }
        public void Equipment(object sender, RoutedEventArgs e)
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
