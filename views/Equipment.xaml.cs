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
using static Space_Shooters.classes.General.User_DataHandling.UserModels;
using System.Windows.Shapes;
using Space_Shooters.classes.General.User_DataHandling;

namespace Space_Shooters.views
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
            LevelChecker();

            PlayerHPHandler playerHPHandler = new();
            PlayerDamageHandler playerDamageHandler = new();
            PlayerASHandler playerASHandler = new();
            PlayerMSHanlder playerMSHanlder = new();
            PlayerLevelHandler playerLevelHandler = new();
            PlayerLPHandler playerLPHandler = new();

            pbLevelProgress.DataContext = playerLPHandler;
            pbLevelProgress.Maximum = PlayerLPHandler.TillLevelUp;
            tbCurrentLevel.DataContext = playerLevelHandler;
            tbNextLevel.Text = $"{Convert.ToInt32(tbCurrentLevel.Text) + 1}";
            tbAS.DataContext = playerASHandler;
            tbHealth.DataContext = playerHPHandler;
            tbDamage.DataContext = playerDamageHandler;
            tbMS.DataContext = playerMSHanlder;

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
        private void LevelChecker()
        {
            if(_UserModel.UserStat.Level >= 5)
            {
                boLvl5.Visibility = Visibility.Collapsed;
                if (_UserModel.UserStat.Level >= 10)
                {
                    boLvl10.Visibility = Visibility.Collapsed;
                    if (_UserModel.UserStat.Level >= 20)
                    {
                        boLvl20.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        private void ItemSlotOne(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("test");
        }
        private void ItemSlotTwo(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("test");
        }
        private void ItemSlotThree(object sender, MouseButtonEventArgs e)
        {

        }
        private void ItemSlotFour(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
