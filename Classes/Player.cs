﻿using Microsoft.Graphics.Canvas;
using System.Drawing;
using System.Linq;

namespace LudoGame.Classes
{
    internal class Player

    {
        public GameRace race;
        public GamePiece[] GamePieces { get; set; }
        public GameTile HomeTile { get; set; }

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
            int result = Dice.ThrowDice();
        }

        public void MouseClick(Point mousePosition)
        {
            //mousePosition
        }
    }
}