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
            else if (gamePiece.race == gameTile.raceHome || gameTile.raceHome == GameRace.None)
            {
                gamePiece.drawable.Position = gameTile.Position;
                gamePiece.atHomePosition = false;
            }
        }

        /// <summary>
        /// Calculate positions and draw game pieces on homebase
        /// </summary>
        //public static GamePiece[] InitializeGamePieces(GameTile[] gameTiles)
        //{
        //    List<GamePiece> gamePieces = new List<GamePiece>();

        //    GameRace race1 = (GameRace)1;
        //    GameRace race2 = (GameRace)2;
        //    GameRace race3 = (GameRace)3;
        //    GameRace race4 = (GameRace)4;
        //    if (gamePieces.Count == 0)
        //    {
        //        float offset = 50;

        //        //gamePieces.Add(race1, new List<GamePiece>());
        //        //gamePieces.Add(race2, new List<GamePiece>());
        //        //gamePieces.Add(race3, new List<GamePiece>());
        //        //gamePieces.Add(race4, new List<GamePiece>());

        //        for (int i = 0; i < gameTiles.Count(); i++)
        //        {
        //            if (gameTiles[i].previousTile == null)
        //            {
        //                if (gameTiles[i].raceHome == race1)
        //                {
        //                    gamePieces.AddRange(CreateGamePieces(race1, "redGamePiece", offset, gameTiles, i));
        //                }
        //                else if (gameTiles[i].raceHome == race2)
        //                {
        //                    gamePieces.AddRange(CreateGamePieces(race2, "greenGamePiece", offset, gameTiles, i));
        //                }
        //                else if (gameTiles[i].raceHome == race3)
        //                {
        //                    gamePieces.AddRange(CreateGamePieces(race3, "yellowGamePiece", offset, gameTiles, i));
        //                }
        //                else if (gameTiles[i].raceHome == race4)
        //                {
        //                    gamePieces.AddRange(CreateGamePieces(race4, "blueGamePiece", offset, gameTiles, i));
        //                }

        //            }
        //        }
        //    }

        //    return gamePieces.ToArray();
        //}

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
        private void ChangeTile(GamePiece gamePiece, int diceResult)
        {
            if (atHomePosition == true)//checks if it tries to leave its home
            {
                if (diceResult == 1 || diceResult == 6)// can only leave home at 1 or 6
                {
                    tile = tile.nextTile;//puts its next tile as tile    
                    System.Threading.Thread.Sleep(1000);// waits 1 second 
                    gamePiece.drawable.Position = tile.Position; // changes its possition
                    atHomePosition = false;//It leaves its home
                }
            }
            else
            {
                System.Threading.Thread.Sleep(1000);// waits 1 second 
                tile = tile.nextTile;//puts its next tile as tile    
                gamePiece.drawable.Position = tile.Position; // changes its possition
            }
        }
    }
}