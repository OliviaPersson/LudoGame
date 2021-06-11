using Microsoft.Graphics.Canvas;
using System.Drawing;
using System.Linq;

namespace LudoGame.Classes
{
    public class Player
    {
        public GameRace race;
        public GamePiece[] GamePieces { get; set; }
        public GameTile baseTile { get; set; }
        public int finishedPieces = 0; //Count finished gamepieces

        public Player(GameRace race, CanvasBitmap gamePieceSprite, float inHomeOffset, GameTile[] gameTiles)
        {
            this.race = race;
            for (int i = 0; i < gameTiles.Count(); i++)
            {
                if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == race)
                {
                    baseTile = gameTiles[i];
                    GamePieces = GamePiece.CreateGamePieces(race, gamePieceSprite, inHomeOffset, baseTile);
                    break;
                }
            }
        }

        /// <summary>
        /// Tries to move the game piece by using the dice roll,
        /// if no piece can move it will end the turn
        /// </summary>
        /// <param name="piece"></param>
        public bool MovePiece(GamePiece piece)
        {
            int dice = Dice.DiceSave;
            if (dice != 0)
            {
                if (piece.CheckAvailableMoves(dice, this))
                {
                    piece.moveToTile = piece.tempTile;
                    if (dice != 6)
                    {
                        Turn.EndTurn();
                    }
                    Dice.UseDice();
                    return true;
                }
                else
                {
                    bool otherPieceCanMove = false;
                    foreach (GamePiece gamePiece in this.GamePieces)
                    {
                        if (gamePiece.CheckAvailableMoves(dice, this))
                        {
                            otherPieceCanMove = true;
                        }
                    }

                    if (!otherPieceCanMove)
                    {
                        Turn.EndTurn();
                        Dice.UseDice();
                    }

                }
            }
            return false;
        }
    }
}