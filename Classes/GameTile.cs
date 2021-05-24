using System.Numerics;

namespace LudoGame.Classes
{
    internal class GameTile
    {
        public bool isHomeTile = false;
        private GameRace raceHome;
        public Drawable drawable;

        public GameTile nextTile;
        public GameTile nextHomeTile;
        public GameTile previousTile;

        public Vector2 Position
        {
            get
            {
                return drawable.Position;
            }
        }

        public GameTile(Drawable drawable, GameRace raceHome)
        {
            this.drawable = drawable;
            this.raceHome = raceHome;

            if (raceHome != GameRace.None)
            {
                isHomeTile = true;
            }
        }

        public GameTile GetNextTile(GameRace gameRace)
        {
            if (isHomeTile && gameRace == raceHome)
            {
                return nextHomeTile;
            }
            else
            {
                return nextTile;
            }
        }
    }
}