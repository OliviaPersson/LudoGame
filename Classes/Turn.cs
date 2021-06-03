using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame.Classes
{
    public static class Turn
    {
        public static bool activeTurn = false;
        public static int activePplayerIndex = 0;
        public static Player activePlayer = GameEngine.players[activePplayerIndex];

        public static void CheckTurn()
        {
            if (!activeTurn)
            {
                activeTurn = true;
                activePlayer = GameEngine.players[activePplayerIndex];
                if(activePlayer.isHumanPlayer)
                {
                    activePlayer.isPlaying = true;

                }
                else
                {
                    StartAIRound();
                }
            }
        }

        public static void EndTurn()
        {
            activePlayer.isPlaying = false;
            if(activePplayerIndex == 3)
            {
                activePplayerIndex = 0;
            }
            else
            {
                activePplayerIndex++;
            }
            activeTurn = false;

        }

        public static void StartAIRound()
        {
            activePlayer = GameEngine.players[activePplayerIndex];
            activePlayer.isPlaying = true;
            GamePiece[] gamePieces = activePlayer.GamePieces;

            int number = Dice.randomNum();
            AIRound(gamePieces, number);
           

        }

        private static void AIRound(GamePiece[] gamePieces, int number)
        {
            Random num = new Random();
            int selectedGamepiece = num.Next(0, gamePieces.Length);

            GamePiece.CheckAvailableMoves(number, gamePieces[selectedGamepiece], activePlayer);
            bool pieceWasMoved = GamePiece.MoveToGameTile(number, gamePieces[selectedGamepiece]);

            if (pieceWasMoved)
            {
                EndTurn();
            }
            else
            {
                if (gamePieces.Length == 1)
                {
                    EndTurn();
                }
                else
                {
                    gamePieces = gamePieces.Where(w => w != gamePieces[selectedGamepiece]).ToArray();
                    AIRound(gamePieces, number);
                }
            }
        }
    }
}
