using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace LudoGame.Classes
{
    public class GamePiece
    {
        public GameTile tile;
        public GameRace race;
        public bool shield;
        public bool atHomePosition = true;
        public Vector2 homePosition;
        public Drawable drawable;

        public bool isAvailableMove = false;
        public bool IsOccupied = false;


        public GamePiece(GameRace gameRace, Vector2 offset, GameTile tile, Drawable drawable)
        {
            this.race = gameRace;
            this.tile = tile;
            this.drawable = drawable;
            this.homePosition = tile.Position + offset;
        }

        public static void MoveToGameTile(int diceResult, GamePiece gamePiece)
        {
            for (int i = 0; i < diceResult; i++) // calls the change tile funk or each number on the dice
            {
                gamePiece.ChangeTile(gamePiece, diceResult);// Changes tile the amount of times the loop goes.
            }
        }

        public static void MovePiece(GamePiece gamePiece, GameTile gameTile)
        {
            if (gameTile.previousTile == null && gamePiece.atHomePosition == false && gamePiece.race == gameTile.raceHome)
            {
                gamePiece.drawable.Position = gamePiece.homePosition;
            }
            else if(gamePiece.race == gameTile.raceHome || gameTile.raceHome == GameRace.None)
            {
                gamePiece.drawable.Position = gameTile.Position;
                gamePiece.atHomePosition = false;
            }
        }

        /// <summary>
        /// Checks if gamePiece is red and change isHoover in drawable class
        /// </summary>
        public static void Hover(GamePiece gamePiece, bool isHover)
        {
            if (gamePiece.race == GameRace.Red)
            {
                gamePiece.drawable.isHover = isHover;
            }
        }
     
        public static GamePiece[] CreateGamePieces(GameRace race, CanvasBitmap sprite, float offset, GameTile gameTile, CanvasBitmap highlightSprite)
        {
            GamePiece[] gamePieces = {
            CreateGamePiece(race, sprite, new Vector2(offset,offset), gameTile, highlightSprite),
            CreateGamePiece(race, sprite, new Vector2(-offset, -offset), gameTile, highlightSprite),
            CreateGamePiece(race, sprite, new Vector2(offset, -offset), gameTile, highlightSprite),
            CreateGamePiece(race, sprite, new Vector2(-offset, offset), gameTile, highlightSprite) };
            return gamePieces;
        }

        private static GamePiece CreateGamePiece(GameRace race, CanvasBitmap sprite, Vector2 offsetPosition, GameTile gameTile, CanvasBitmap highlightSprite)
        {
            Drawable draw = new Drawable(sprite, gameTile.Position + offsetPosition, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), highlightSprite);
            GameEngine.drawables.Add(draw);
            return new GamePiece(race, offsetPosition, gameTile, draw);
        }

        private void ChangeTile(GamePiece gamePiece, int diceResult)
        {
            if (atHomePosition == true)//checks if it tries to leave its home
            {
                if (diceResult == 1 || diceResult == 6)// can only leave home at 1 or 6
                {
                    tile = tile.nextTile; //puts its next tile as tile    
                    System.Threading.Thread.Sleep(500);// waits 1 second 
                    gamePiece.drawable.Position = tile.Position; // changes its possition
                    atHomePosition = false;//It leaves its home
                }
            }
            else
            {
                System.Threading.Thread.Sleep(500);// waits 1 second 
                tile = tile.nextTile;//puts its next tile as tile    
                gamePiece.drawable.Position = tile.Position; // changes its possition
            }
        }
    }
}