using Microsoft.Graphics.Canvas;
using System.Drawing;
using System.Linq;

namespace LudoGame.Classes
{
    public class Player
    {
        public GameRace race;
        public GamePiece[] GamePieces { get; set; }
        public GameTile HomeTile { get; set; }
        public bool turnDone = false;
        public int finishedPieces = 0; //Count finished gamepieces

        public Player(GameRace race, CanvasBitmap gamePieceSprite, float inHomeOffset, GameTile[] gameTiles)
        {
            this.race = race;
            for (int i = 0; i < gameTiles.Count(); i++)
            {
                if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == race)
                {
                    HomeTile = gameTiles[i];
                    GamePieces = GamePiece.CreateGamePieces(race, gamePieceSprite, inHomeOffset, HomeTile);
                    break;
                }
            }
        }

        public void RollDice()
        {
            int result = Dice.RollDice();
        }

        public void MouseClick(Point mousePosition)
        {
            //mousePosition
        }

        public void MovePiece(GamePiece piece)
        {
            if (piece.CheckAvailableMoves(Dice.DiceSave, this))
            {
                piece.moveToTile = piece.tempTile;
                if (Dice.DiceSave != 6)
                {
                    turnDone = true;
                }
                Dice.DiceSave = 0;
            }

        }
    }
}