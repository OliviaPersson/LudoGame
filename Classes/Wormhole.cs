using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;
using System.Numerics;

namespace LudoGame.Classes
{
    public class Wormhole
    {
        public Vector2 Position { get { return drawable.Position; } set { drawable.Position = value; } }
        public static GameTile StartPosition  {  get; set;  }
        public static GameTile EndPosition { get; set; }
        private static CanvasBitmap _sprite { get; set; }
        private  static GameTile[] _gameTiles { get; set; }
        static Random rnd = new Random();
        public Drawable drawable;
        public GameTile tile;
        public Wormhole(Drawable drawable, GameTile tile)
        {
            
            
            this.drawable = drawable;
            this.tile = tile; 
        }
        public static Wormhole CreateWormHole(GameTile[] gametiles, CanvasBitmap sprite) {
            _sprite = sprite;
            _gameTiles = gametiles;
            int ran;
            do {
                ran = rnd.Next(gametiles.Length);
                StartPosition = gametiles[ran];
            } while (gametiles[ran].raceHome == (GameRace)1 || gametiles[ran].raceHome == (GameRace)2||gametiles[ran].raceHome == (GameRace)3|| gametiles[ran].raceHome == (GameRace)4);
            
           return SpawnWormHole(0, sprite, StartPosition);
        }
        public static  void MoveWormhole()
        {
          
           
            
            int ran;
            do
            {
                ran = rnd.Next(_gameTiles.Length);
                StartPosition = _gameTiles[ran];
            } while (_gameTiles[ran].raceHome == (GameRace)1 || _gameTiles[ran].raceHome == (GameRace)2 || _gameTiles[ran].raceHome == (GameRace)3 || _gameTiles[ran].raceHome == (GameRace)4);
            GameEngine.wormHole.Position = _gameTiles[ran].Position;
            GameEngine.wormHole.Position += Vector2Math.Normalized(_gameTiles[ran].Position) *50* (float)GameEngine.GameCanvas.TargetElapsedTime.TotalSeconds;
            
        }
        public static void CheckIfHitWormhole(GamePiece gamePiece)
        {
            int ran;
            do
            {
                ran = rnd.Next(_gameTiles.Length);
                StartPosition = _gameTiles[ran];
            } while (_gameTiles[ran].raceHome == (GameRace)1 || _gameTiles[ran].raceHome == (GameRace)2 || _gameTiles[ran].raceHome == (GameRace)3 || _gameTiles[ran].raceHome == (GameRace)4);

            foreach (GamePiece otherGamePiece in GameEngine.gamePieces)
            {
               
                if (otherGamePiece.tile == StartPosition)
                {
                     otherGamePiece.moveToTile = _gameTiles[ran]; 
                }
                else if(gamePiece.tile == StartPosition)
                {
                    gamePiece.moveToTile = _gameTiles[ran];
                }

            }
        }
        private static Wormhole SpawnWormHole(GameRace race, CanvasBitmap sprite,  GameTile baseTile)
        {
            Drawable draw = new Drawable(sprite, baseTile.Position, 0.4f, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale));
            GameEngine.drawables.Add(draw);
            return new Wormhole(draw, baseTile);
        }

    }
}