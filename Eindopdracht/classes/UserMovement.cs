using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Eindopdracht.classes
{
    public class UserMovement
    {
        public static void MoveLeft(Border user)
        {
            // Move the player to the left
            user.Margin = new System.Windows.Thickness(user.Margin.Left - 10, user.Margin.Top, 0, 0);
        }
        public static void MoveRight(Border user)
        {
            // Move the player to the right
            user.Margin = new System.Windows.Thickness(user.Margin.Left + 10, user.Margin.Top, 0, 0);
        }
    }
}
