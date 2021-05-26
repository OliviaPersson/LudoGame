using System.Numerics;

namespace LudoGame.Classes
{
    public class GameTile
    {
        private GameRace raceHome;
        public Drawable drawable;

        public GameTile nextTile;
        public GameTile nextHomeTile;
        public GameTile previousTile
        {
            get { return _previousTile; }
            set
            {
                if (value != null)
                {
                    _previousTile = value;

                    if (raceHome != 0&& _previousTile.raceHome == raceHome)
                    {
                        _previousTile.nextHomeTile = this;
                    }
                    else
                    {
                        _previousTile.nextTile = this;
                    }

                }
            }
        }
        private GameTile _previousTile;

        public Vector2 Position
        {
            get
            {
                return drawable.Position;
            }
        }

        public GameTile(Drawable drawable, GameRace raceHome, GameTile previousTile)
        {
            this.drawable = drawable;
            this.raceHome = raceHome;
            this.previousTile = previousTile;

        }

        public GameTile GetNextTile(GameRace gameRace)
        {
            if (gameRace == raceHome)
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