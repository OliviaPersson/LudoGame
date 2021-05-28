using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame.Classes
{
    class AIPlayer
    {   
        //Color for wich team the ai is playing as
        public Player Player { get; set; }

        public AIPlayer(Player player)
        {
            Player = player;
        }

        public void Play()
        {
            
        }
    }
}
