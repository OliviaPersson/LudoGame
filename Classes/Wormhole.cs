using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;


namespace LudoGame.Classes
{
    public static class Wormhole
    {
        public static Vector2 Position { get { return drawable.Position; } set { drawable.Position = value; } }
        public static GameTile StartPosition  {  get; set;  }
        public static GameTile EndPosition { get; set; }
        private  static GameTile[] _gameTiles { get; set; }
        static Random rnd = new Random();
        public static Drawable drawable;
        public static void CreateWormHole(GameTile[] gametiles, CanvasBitmap sprite)
        {

            drawable = new Drawable(sprite, Vector2.Zero, 0.4f, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale));
            GameEngine.drawables.Add(drawable);

            _gameTiles = gametiles;
            StartPosition = gametiles[0];
            NewPosition(10);
        }

        private static void NewPosition(int advanceStartPos)
        {
            for (int i = 0; i < advanceStartPos; i++)
            {
                StartPosition = StartPosition.GetNextTile(0);
            }
            EndPosition = StartPosition;
            for (int i = 0; i > -10; i--)
            {
                EndPosition = EndPosition.previousTile;
            }

            int ran = rnd.Next(21);
            for (int i = 0; i < ran; i++)
            {
                EndPosition = EndPosition.GetNextTile(0);
            }
        }
    
    public static  void MoveWormhole()
        {
            int ran = rnd.Next(30);
            NewPosition(ran);
            Position = StartPosition.Position;
            foreach (GamePiece otherGamePiece in GameEngine.gamePieces)
            {
                if (otherGamePiece.tile == StartPosition)
                {
                    WarpPiece(otherGamePiece);
                }
            }
        }

        private static void WarpPiece(GamePiece otherGamePiece)
        {
            otherGamePiece.moveToTile = EndPosition;
            otherGamePiece.tile = EndPosition;
            otherGamePiece.Position = EndPosition.Position;
            GamePiece.CheckIfHitGamePiece(otherGamePiece);
        }

        public static void CheckIfHitWormhole(GamePiece gamePiece)
        {
            if (gamePiece.tile == StartPosition)
            {
                WarpPiece(gamePiece);
            }
            
        }
    }
}