using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Space_Shooters.classes.Game.Game_VariableHandling.GameTick;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Space_Shooters.classes.Game.Game_CollisionHandling
{
    internal class CollisionLoop(Grid mainGrid)
    {
        public async void Start()
        {
            while (true)
            {
                if (!Paused)
                {
                    Collisions.CheckCollision(mainGrid);
                }
                await Task.Delay(50); // Adjust delay as needed
             }
        }
    }
}
