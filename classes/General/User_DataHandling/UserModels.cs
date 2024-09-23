using Space_Shooters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Shooters.classes.General.User_DataHandling
{
    internal class UserModels
    {
        internal static UserModel _UserModel;
        internal class UserModel
        {
            internal string Skin { get; set; }
            internal UserStat UserStat { get; set; }
            internal UserGameStat UserGameStat { get; set; }

        }
    }
}
