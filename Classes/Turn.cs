using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame.Classes
{
    public static class Turn
    {
        public static GameRace activePlayer = (GameRace)1;

        /// <summary>
        /// Check if a player is done and advances to the next player if it is
        /// </summary>
        public static void CheckTurn()
        {
            if (GameEngine.players != null)
            {
                Player currentPlayer = GameEngine.players[(int)activePlayer - 1];
                if (!currentPlayer.baseTile.drawable.isHover)
                {
                    currentPlayer.baseTile.drawable.isHover = true;
                }

                if (currentPlayer.GamePieces.Length == 0) // skips a player that have no game pieces
                {
                    EndTurn();
                    return;
                }

                if (currentPlayer != GameEngine.player)
                {
                    AIPlay(currentPlayer);
                }
            }
        }

        /// <summary>
        /// Ends the active players turn
        /// </summary>
        public static void EndTurn()
        {
            
            if (activePlayer == (GameRace)4)
            {
                Wormhole.MoveWormhole();
                activePlayer = (GameRace)1;
            }
            else
            {
                activePlayer++;
            }


            if (GameEngine.players[(int)activePlayer - 1] == GameEngine.player)
            {
                GameEngine.currentGameState = GameState.PlayerPlaying;
            }
            else
            {
                GameEngine.currentGameState = GameState.AIPlaying;
            }

            foreach (GameTile tile in GameEngine.GameTiles)
            {
                tile.drawable.isHover = false;
            }
        }

        /// <summary>
        /// Lets the current AI play
        /// </summary>
        /// <param name="currentPlayer"></param>
        public static void AIPlay(Player currentPlayer)
        {
            foreach (AIPlayer AI in GameEngine.aIPlayers)
            {
                if (AI.Player == currentPlayer)
                {
                    AI.Play(); // make so it only sets turnDone true if it is truly done
                }
            }
        }
    }
}
