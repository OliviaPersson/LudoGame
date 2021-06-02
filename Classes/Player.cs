﻿using Microsoft.Graphics.Canvas;
using System.Drawing;
using System.Linq;

namespace LudoGame.Classes
{
    public class Player
    {
        public GameRace race;
        public GamePiece[] GamePieces { get; set; }
        public GameTile HomeTile { get; set; }


        public Player(GameRace race, CanvasBitmap gamePieceSprite, float inHomeOffset, GameTile[] gameTiles, CanvasBitmap highlightSprite)
        {
            this.race = race;
            for (int i = 0; i < gameTiles.Count(); i++)
            {
                if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == race)
                {
                    HomeTile = gameTiles[i];
                    GamePieces = GamePiece.CreateGamePieces(race, gamePieceSprite, inHomeOffset, HomeTile, highlightSprite);
                    break;
                }
            }
        }

        public void RollDice()
        {
            int result = Dice.randomNum();
        }

        public void MouseClick(Point mousePosition)
        {
            //mousePosition
        }


    }
}