using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using Space_Shooters.classes.Game.Game_DataHandling;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;   

namespace Space_Shooters.classes.Game.Game_UIHandling
{
    internal class CenterTextHandling
    {
        public static void UpdateCenterText(string text)
        {
            BindingOperations.ClearBinding(_WindowModel.CenterBlock, OutlinedTextControl.TextProperty);
            _WindowModel.CenterBlock.Visibility = Visibility.Visible;
            _WindowModel.CenterBlock.Text = text?.ToString() ?? string.Empty;
        }
    }
}
