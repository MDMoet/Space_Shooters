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
using System.Windows.Shapes;

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for GameMenu.xaml
    /// </summary>
    public partial class GameMenu : UserControl
    {
        private readonly ViewHandler VarViewHandler;
        public GameMenu(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            this.VarViewHandler = VarViewHandler;
        }
        public void StartGame(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToGame();
        }
        public void Equipment(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToSkins();
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
