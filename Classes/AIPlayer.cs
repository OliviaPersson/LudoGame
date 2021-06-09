using System;
using System.Linq;

namespace LudoGame.Classes
{
    public class AIPlayer
    {
        // The Player that this AI is controlling
        public Player Player { get; set; }

        public AIPlayer(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// This is where the AI plays it's turn
        /// </summary>
        public void Play()
        {
            int roll = Dice.RollDice();
            
            // Move the first piece that can move for now
            // It will always move the first piece as it always has a valid move.
            // If time allows improve with smart choises.
            // It also doens't play again if it rolled a 6
            foreach (GamePiece piece in Player.GamePieces) 
            {
                if (piece.CheckAvailableMoves(roll, Player))
                {
                    Player.MovePiece(piece);
                    break;
                }
            }


            //Random num = new Random();
            //int selectedGamepiece = num.Next(0, gamePieces.Length);

            //GamePiece.CheckAvailableMoves(number, gamePieces[selectedGamepiece], Player);
            //bool pieceWasMoved = GamePiece.MoveToGameTile(number, gamePieces[selectedGamepiece]);

            //if (pieceWasMoved)
            //{
            //    Turn.EndTurn();
            //}
            //else
            //{
            //    if (gamePieces.Length == 1)
            //    {
            //        Turn.EndTurn();
            //    }
            //    else
            //    {
            //        gamePieces = gamePieces.Where(w => w != gamePieces[selectedGamepiece]).ToArray();
            //        AIRound(gamePieces, number);
            //    }
            //}
        }
    }
}