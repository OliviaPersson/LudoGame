using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace LudoGame.Classes
{
    public class GamePiece
    {
        //public GameTile Position;
        public string color;
        public bool Shield;
        public Vector2 homePosition;
        public string spriteName;

        public GamePiece(string color, string spriteName, Vector2 homePosition)
        {
            this.color = color;
            this.spriteName = spriteName;
            this.homePosition = homePosition;

        }
    }
}
