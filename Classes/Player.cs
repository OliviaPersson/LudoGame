using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame.Classes
{
    class Player

    {
        public GameRace race;
        public GamePiece[] GamePieces = new GamePiece[4];
        public CanvasBitmap GamePiceSprite;
        public void RollDice()
        {
            int result = Dice.randomNum();
        }
        public void MouseClick(Point mousePosition)
        {
            //mousePosition
        }
    }
  
}
