using System;
using System.Linq;
using System.Collections.Generic;

namespace LudoGame.Classes
{
    public class AIPlayer
    {
        // The Player that this AI is controlling
        public Player Player { get; set; }
        Random num = new Random();
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
            List<GamePiece> piecesToMove = Player.GamePieces.ToList();
            
            bool moved;

            do
            {
                int selectedGamepiece = num.Next(0, piecesToMove.Count);
                if (piecesToMove.Count != 0)
                {
                    moved = Player.MovePiece(piecesToMove[selectedGamepiece]);
                    if (moved)
                    {
                        break;
                    }
                    else
                    {
                        piecesToMove.RemoveAt(selectedGamepiece);
                    }
                }
                else
                {
                    break;
                }
            } while (!moved);

            

            // Move the first piece that can move for now
            // It will always move the first piece as it always has a valid move.
            // If time allows improve with smart choises.
            // It also doens't play again if it rolled a 6
            //foreach (GamePiece piece in Player.GamePieces)
            //{
            //    if (Player.MovePiece(piece))
            //    {
            //        break; // break out of loop because a piece was able to move
            //    }
            //}


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