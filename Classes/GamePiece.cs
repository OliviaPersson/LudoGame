using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace LudoGame.Classes
{
    public class GamePiece
    {
        public static Dictionary<string, List<GamePiece>> gamePieces = new Dictionary<string, List<GamePiece>>();
        public GameTile tile;
        public string color;
        public bool Shield;
        public Vector2 homePosition;
        public string spriteName;

        public GamePiece(string color, string spriteName, Vector2 offset, GameTile tile)
        {
            this.color = color;
            this.spriteName = spriteName;
            this.tile = tile;

            this.homePosition = tile.Position + offset;

        }

        /// <summary>
        /// Calculate positions and draw game pieces on homebase
        /// </summary>
        public static void InitializeGamePieces(GameTile[] gameTiles)
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
                    gamePieces["red"].Add(new GamePiece("red", "redGamePiece", new Vector2(offset, offset), gameTiles[i]));
                    gamePieces["red"].Add(new GamePiece("red", "redGamePiece", new Vector2(-offset, -offset), gameTiles[i]));
                    gamePieces["red"].Add(new GamePiece("red", "redGamePiece", new Vector2(offset, -offset), gameTiles[i]));
                    gamePieces["red"].Add(new GamePiece("red", "redGamePiece", new Vector2(-offset, offset), gameTiles[i]));
                }
                if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == GameRace.Blue)
                {
                    gamePieces["blue"].Add(new GamePiece("blue", "blueGamePiece", new Vector2(offset, offset), gameTiles[i]));
                    gamePieces["blue"].Add(new GamePiece("blue", "blueGamePiece", new Vector2(-offset, -offset), gameTiles[i]));
                    gamePieces["blue"].Add(new GamePiece("blue", "blueGamePiece", new Vector2(offset, -offset), gameTiles[i]));
                    gamePieces["blue"].Add(new GamePiece("blue", "blueGamePiece", new Vector2(-offset, offset), gameTiles[i]));
                }
                if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == GameRace.Yellow)
                {
                    gamePieces["yellow"].Add(new GamePiece("yellow", "yellowGamePiece", new Vector2(offset, offset), gameTiles[i]));
                    gamePieces["yellow"].Add(new GamePiece("yellow", "yellowGamePiece", new Vector2(-offset, -offset), gameTiles[i]));
                    gamePieces["yellow"].Add(new GamePiece("yellow", "yellowGamePiece", new Vector2(offset, -offset), gameTiles[i]));
                    gamePieces["yellow"].Add(new GamePiece("yellow", "yellowGamePiece", new Vector2(-offset, offset), gameTiles[i]));
                }
                if (gameTiles[i].previousTile == null && gameTiles[i].raceHome == GameRace.Green)
                {
                    gamePieces["green"].Add(new GamePiece("green", "greenGamePiece", new Vector2(offset, offset), gameTiles[i]));
                    gamePieces["green"].Add(new GamePiece("green", "greenGamePiece", new Vector2(-offset, -offset), gameTiles[i]));
                    gamePieces["green"].Add(new GamePiece("green", "greenGamePiece", new Vector2(offset, -offset), gameTiles[i]));
                    gamePieces["green"].Add(new GamePiece("green", "greenGamePiece", new Vector2(-offset, offset), gameTiles[i]));
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
}
