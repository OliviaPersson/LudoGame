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


        public static void CheckTurn()
        {
            if (GameEngine.players != null)
            {
                Player currentPlayer = GameEngine.players[(int)activePlayer - 1];
                if (currentPlayer.turnDone == true)
                {
                    currentPlayer.turnDone = false;
                    EndTurn();
                }
                else
                {
                    AIPlay(currentPlayer);
                }
            }
        }

        public static void EndTurn()
        {
            if (activePlayer == (GameRace)4)
            {
                activePlayer = (GameRace)1;
            }
            else
            {
                activePlayer++;
            }
            GameEngine.players[(int)activePlayer - 1].turnDone = false;
            

            if (GameEngine.players[(int)activePlayer - 1] == GameEngine.player)
            {
                GameEngine.currentGameState = GameState.PlayerPlaying;
            }else
            {
                GameEngine.currentGameState = GameState.AIPlaying;
            }

            foreach (GameTile tile in GameEngine.GameTiles)
            {
                tile.drawable.isHover = false;
            }

            GameEngine.players[(int)activePlayer - 1].GamePieces[0].baseTile.drawable.isHover = true;
        }

        public static void AIPlay(Player currentPlayer)
        {
            foreach (AIPlayer AI in GameEngine.aIPlayers)
            {
                if (AI.Player == currentPlayer)
                {
                    AI.Player.turnDone = false;
                    AI.Play();
                    AI.Player.turnDone = true;
                }
            }
        }
    }
}
