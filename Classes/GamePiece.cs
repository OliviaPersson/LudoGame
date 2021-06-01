using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace LudoGame.Classes
{
    public class GamePiece
    {
        //public static Dictionary<GameRace, List<GamePiece>> gamePieces = new Dictionary<GameRace, List<GamePiece>>();
        public GameTile tile;

        public GameRace race;
        public bool shield;
        public bool atHomePosition = true;
        public Vector2 homePosition;
        public Drawable drawable;

        public GamePiece(GameRace gameRace, Vector2 offset, GameTile tile, Drawable drawable)
        {
            this.race = gameRace;
            this.tile = tile;
            this.drawable = drawable;
            this.homePosition = tile.Position + offset;
        }

        public void MovePiece(int diceThrow)
        {
            for (int i = 0; i < diceThrow; i++)
            {
                tile = tile.GetNextTile(race);
                drawable.Position = tile.Position;
            }
        }

        public static GamePiece[] CreateGamePieces(GameRace race, CanvasBitmap sprite, float offset, GameTile gameTile)
        {
            GamePiece[] gamePieces = {
            CreateGamePiece(race, sprite, new Vector2(offset,offset), gameTile),
            CreateGamePiece(race, sprite, new Vector2(-offset, -offset), gameTile),
            CreateGamePiece(race, sprite, new Vector2(offset, -offset), gameTile),
            CreateGamePiece(race, sprite, new Vector2(-offset, offset), gameTile) };
            return gamePieces;
        }

        private static GamePiece CreateGamePiece(GameRace race, CanvasBitmap sprite, Vector2 offsetPosition, GameTile gameTile)
        {
            Drawable draw = new Drawable(sprite, gameTile.Position + offsetPosition, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale));
            GameEngine.drawables.Add(draw);
            return new GamePiece(race, offsetPosition, gameTile, draw);
        }
    }
}