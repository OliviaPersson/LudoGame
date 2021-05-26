using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame.Classes
{
    class Player

    {
        // Color for the team /color you are is to be added. 
        public GamePiece[] GamePieces = new GamePiece[4];
        public CanvasBitmap GamePiceSprite;
        public void RollDice()
        {
            int result = Dice.randomNum();
        }
    }
  
}
