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
        public static GameRace activePlayer = (GameRace)1;


        public static void CheckTurn()
        {
            if (!activeTurn && GameEngine.players != null)
            {
                activeTurn = true;
                Player currentPlayer = GameEngine.players[(int)activePlayer - 1];
                if (currentPlayer == GameEngine.player)
                {
                    currentPlayer.turnDone = false;
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
