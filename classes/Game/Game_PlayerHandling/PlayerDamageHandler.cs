using Space_Shooters.classes.Game.Game_EntityHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Space_Shooters.classes.Game.Game_VariableHandling.PassableVariables;
using static Space_Shooters.classes.Game.Game_PlayerHandling.PlayerHPHandler;
using static Space_Shooters.views.Game;
using Space_Shooters.classes.Game.Game_UIHandling;
using Space_Shooters.Models;
using System.Windows.Controls;

namespace Space_Shooters.classes.Game.Game_PlayerHandling
{
    internal class PlayerDamageHandler
    {
        internal static UserStat UserStat = _UserModel.UserStat;
        internal static void HandlePlayerDamage(int damage)
        {
            if (PlayerHP - damage * 10 <= 0)
            {
                PlayerHP = 0;
                _UserModel.UserGameStat.Deaths++;
                GameOver();
            }
            else
            {
                DecreaseHP(damage * 10);
            }
        }
    }
}
