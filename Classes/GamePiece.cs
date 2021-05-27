using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Windows.Foundation;

namespace LudoGame.Classes
{
    public class GamePiece
    {
        public static Dictionary<string, List<GamePiece>> gamePieces = new Dictionary<string, List<GamePiece>>();
        public GameTile tile;
        public string color;
        public bool Shield;
        public string spriteName;
        public int ID;
        public bool atHomePosition = true;
        public Vector2 homePosition;

        public GamePiece(int ID, string color, string spriteName, Vector2 offset, GameTile tile)
        {
            this.color = color;
            this.spriteName = spriteName;
            this.tile = tile;
            this.ID = ID;

            this.homePosition = tile.Position + offset;

        }

        /// <summary>
        /// Calculate positions and draw game pieces on homebase
        /// </summary>
        public static void InitializeGamePieces(GameTile[] gameTiles)
        {
            if(gamePieces.Count == 0)
            {
                float offset = 50;

                gamePieces.Add("red", new List<GamePiece>());
                gamePieces.Add("blue", new List<GamePiece>());
                gamePieces.Add("yellow", new List<GamePiece>());
                gamePieces.Add("green", new List<GamePiece>());

                for (int i = 0; i < gameTiles.Count(); i++)
                {
                    if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == GameRace.Red)
                    {
                        gamePieces["red"].Add(new GamePiece(1, "red", "redGamePiece", new Vector2(offset, offset), gameTiles[i]));
                        gamePieces["red"].Add(new GamePiece(2, "red", "redGamePiece", new Vector2(-offset, -offset), gameTiles[i]));
                        gamePieces["red"].Add(new GamePiece(3, "red", "redGamePiece", new Vector2(offset, -offset), gameTiles[i]));
                        gamePieces["red"].Add(new GamePiece(4, "red", "redGamePiece", new Vector2(-offset, offset), gameTiles[i]));
                    }
                    if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == GameRace.Blue)
                    {
                        gamePieces["blue"].Add(new GamePiece(5, "blue", "blueGamePiece", new Vector2(offset, offset), gameTiles[i]));
                        gamePieces["blue"].Add(new GamePiece(6, "blue", "blueGamePiece", new Vector2(-offset, -offset), gameTiles[i]));
                        gamePieces["blue"].Add(new GamePiece(7, "blue", "blueGamePiece", new Vector2(offset, -offset), gameTiles[i]));
                        gamePieces["blue"].Add(new GamePiece(8, "blue", "blueGamePiece", new Vector2(-offset, offset), gameTiles[i]));
                    }
                    if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == GameRace.Yellow)
                    {
                        gamePieces["yellow"].Add(new GamePiece(9, "yellow", "yellowGamePiece", new Vector2(offset, offset), gameTiles[i]));
                        gamePieces["yellow"].Add(new GamePiece(10, "yellow", "yellowGamePiece", new Vector2(-offset, -offset), gameTiles[i]));
                        gamePieces["yellow"].Add(new GamePiece(11, "yellow", "yellowGamePiece", new Vector2(offset, -offset), gameTiles[i]));
                        gamePieces["yellow"].Add(new GamePiece(12, "yellow", "yellowGamePiece", new Vector2(-offset, offset), gameTiles[i]));
                    }
                    if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == GameRace.Green)
                    {
                        gamePieces["green"].Add(new GamePiece(13, "green", "greenGamePiece", new Vector2(offset, offset), gameTiles[i]));
                        gamePieces["green"].Add(new GamePiece(14, "green", "greenGamePiece", new Vector2(-offset, -offset), gameTiles[i]));
                        gamePieces["green"].Add(new GamePiece(15, "green", "greenGamePiece", new Vector2(offset, -offset), gameTiles[i]));
                        gamePieces["green"].Add(new GamePiece(16, "green", "greenGamePiece", new Vector2(-offset, offset), gameTiles[i]));
                    }
                }

                foreach (string key in gamePieces.Keys)
                {
                    for (int i = 0; i < gamePieces[key].Count; i++)
                    {
                        GameEngine.drawables.Add(new Drawable(GameEngine._sprites[gamePieces[key][i].spriteName], gamePieces[key][i].homePosition, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
                    }
                }
            }
        }

        /*
        public static GamePiece SelectPiece(Point position)
        {
            Point max = new Point(position.X + 25, position.Y + 25);
            Point min = new Point(position.X - 25, position.Y - 25);

            foreach (string key in gamePieces.Keys)
            {
                for (int i = 0; i < gamePieces[key].Count; i++)
                {
                    if (max.X > gamePieces[key][i].tile.Position.X && min.X < gamePieces[key][i].tile.Position.X && max.Y > gamePieces[key][i].tile.Position.Y && min.Y < gamePieces[key][i].tile.Position.Y)
                    {
                        return gamePieces[key][i];
                    }
                    
                }
            }
            return null;
        }
        */
    }
}
