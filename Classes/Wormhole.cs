using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;
using System.Numerics;

namespace LudoGame.Classes
{
    internal class Wormhole
    {
        public static GameTile StartPosition  {  get; set;  }
        public static GameTile EndPosition { get; set; }
        static Random rnd = new Random();
        public static void CreateWormHole(GameTile[] gametiles, CanvasBitmap sprite) {
            int ran = rnd.Next(gametiles.Length);
            StartPosition = gametiles[ran];
            GamePiece.CreateGamePieces(0, sprite, 1f, StartPosition);
        }
        //public static void CheckIfHitWormhole(GamePiece gamePiece)
        //{
        //    foreach (GamePiece otherGamePiece in GameEngine.gamePieces)
        //    {
        //        if (otherGamePiece.tile == gamePiece.tile && otherGamePiece.race != gamePiece.race)
        //        {
        //            otherGamePiece.tile = otherGamePiece.baseTile;
        //            otherGamePiece.moveToTile = otherGamePiece.tile;
        //            otherGamePiece.Position = otherGamePiece.homePosition;
        //            otherGamePiece.atHomePosition = true;
        //        }

        //    }
        //}

        
    }
}