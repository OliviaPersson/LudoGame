using System.Numerics;

namespace LudoGame.Classes
{
    internal class GameTile
    {
        public string NameID { get; private set; }
        public Drawable drawable;

        public Vector2 Position
        {
            get
            {
                return drawable.Position;
            }
        }

        public GameTile(string nameID, Drawable drawable)
        {
            this.NameID = nameID;
            this.drawable = drawable;
        }
    }
}