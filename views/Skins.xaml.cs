using Space_Shooters.classes;
using Space_Shooters.classes.General.User_DataHandling;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Space_Shooters.views
{
    /// <summary>
    /// Interaction logic for Skins.xaml
    /// </summary>
    public partial class Skins : UserControl
    {
        private readonly ViewHandler VarViewHandler;
        public Skins(ViewHandler VarViewHandler)
        {
            InitializeComponent();
            LoadSkins();    
            this.VarViewHandler = VarViewHandler;
        }
        public void LoadSkins()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            // Adjust the path to point to the project directory
            var d = new DirectoryInfo(System.IO.Path.Combine(basePath, "..\\..\\..\\img\\skins\\User_Skins\\"));

            if (!d.Exists)
            {
                // Handle the case where the directory does not exist
                MessageBox.Show($"The directory {d.FullName} does not exist. Please check the path.", "Directory Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (FileInfo fi in d.GetFiles())
            {
                ListBoxItem container = new()
                {
                    Height = 75,
                    Width = 75,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFC900")),
                    Background = Brushes.Transparent,
                    IsHitTestVisible = true,
                    Tag = fi.Name.Remove(fi.Name.Length - 4),
                    Margin = new Thickness(5, 2, 0, 3)
                };

                Image image = new()
                {
                    Width = 70,
                    Height = 70,
                    Source = new BitmapImage(new Uri(System.IO.Path.Combine(basePath, "..\\..\\..\\img\\skins\\User_Skins\\", fi.Name), UriKind.RelativeOrAbsolute)),
                    Margin = new Thickness(0, 2, 0, 0)

                };
                container.Selected += SkinSelect;

                container.Content = image;

                lbSkinsInventory.Items.Add(container);
            }

            imgSelectedSkin.Source = new BitmapImage(new Uri(System.IO.Path.Combine(basePath, "..\\..\\..\\img\\skins\\User_Skins\\" + _UserModel.Skin + ".png"), UriKind.RelativeOrAbsolute));
            tbSkinName.Text = _UserModel.Skin.Replace("_", " ");
            otEquip.Text = "Equipped";
        }

        private void SkinSelect(object sender, RoutedEventArgs e)
        {
            if (sender is ListBoxItem listBoxItem)
            {
                // Handle the button click event
                if(listBoxItem.Tag.ToString() != _UserModel.Skin)
                {
                    otEquip.Text = "Equip";
                }
                else
                {
                    otEquip.Text = "Equipped";
                }
                imgSelectedSkin.Source = ((Image)listBoxItem.Content).Source;
                tbSkinName.Text = listBoxItem.Tag.ToString().Replace("_", " ");
            }
        }
        public void Equip(object sender, RoutedEventArgs e)
        {
            _UserModel.Skin = tbSkinName.Text.Replace(" ", "_");
            otEquip.Text = "Equipped";
            PlayerDataHandling.UpdateSkin(_UserModel.Skin);
        }
        public void Return(object sender, RoutedEventArgs e)
        {
            VarViewHandler.GoToMainMenu();
        }
    }
}
