using System;
using System.Linq;

namespace LudoGame.Classes
{
    public class AIPlayer
    {
        //Color for wich team the ai is playing as
        public Player Player { get; set; }

        public AIPlayer(Player player)
        {
            Player = player;
        }

        public void Play()
        {
            int roll = Dice.RollDice();

            foreach (GamePiece piece in Player.GamePieces) // move the first piece that can move for now. If time allows improve with smart choises.
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