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
        public bool turnDone { get; set; }
        public int finishedPieces = 0; //Count finished gamepieces

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
            int result = Dice.RollDice();
        }

        public void MouseClick(Point mousePosition)
        {
            //mousePosition
        }

        public void MovePiece(GamePiece piece)
        {
            int dice = Dice.DiceSave;
            if (dice != 0)
            {
                if (piece.CheckAvailableMoves(dice, this))
                {
                    piece.moveToTile = piece.tempTile;
                    if (dice != 6)
                    {
                        turnDone = true;
                    }
                    Dice.UseDice();
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
                        turnDone = true;
                        Dice.UseDice();
                    }

                }
            }
        }
    }
}