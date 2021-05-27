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
        public static Dictionary<GameRace, List<GamePiece>> gamePieces = new Dictionary<GameRace, List<GamePiece>>();
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

        public static void MovePiece(int diceResult, int id, GameRace gameRace)
        {

            for (int i = 0; i < diceResult; i++)
            {



            }
        }

        /// <summary>
        /// Calculate positions and draw game pieces on homebase
        /// </summary>
        public static void InitializeGamePieces(GameTile[] gameTiles)
        {
            GameRace race1 = (GameRace)1;
            GameRace race2 = (GameRace)2;
            GameRace race3 = (GameRace)3;
            GameRace race4 = (GameRace)4;
            if (gamePieces.Count == 0)
            {
                float offset = 50;

                gamePieces.Add(race1, new List<GamePiece>());
                gamePieces.Add(race2, new List<GamePiece>());
                gamePieces.Add(race3, new List<GamePiece>());
                gamePieces.Add(race4, new List<GamePiece>());

                for (int i = 0; i < gameTiles.Count(); i++)
                {
                    if (gameTiles[i].previousTile == null)
                    {
                        if (gameTiles[i].raceHome == race1)
                        {
                            AddGamePieces(race1, "redGamePiece", offset, gameTiles, i);
                        }
                        else if (gameTiles[i].raceHome == race2)
                        {
                            AddGamePieces(race2, "greenGamePiece", offset, gameTiles, i);
                        }
                        else if (gameTiles[i].raceHome == race3)
                        {
                            AddGamePieces(race3, "yellowGamePiece", offset, gameTiles, i);
                        }
                        else if (gameTiles[i].raceHome == race4)
                        {
                            AddGamePieces(race4, "blueGamePiece", offset, gameTiles, i);
                        }
 
                    }
                }
            }
        }
        private static void AddGamePieces(GameRace race,string sprite, float offset, GameTile[] gameTiles, int i)
        {
           
            CreateGamePiece(race, sprite, new Vector2(offset,offset), gameTiles, i);
            CreateGamePiece(race, sprite, new Vector2(-offset, -offset), gameTiles, i);
            CreateGamePiece(race, sprite, new Vector2(offset, -offset), gameTiles, i);
            CreateGamePiece(race, sprite, new Vector2(-offset, offset), gameTiles, i);
        }

        private static void CreateGamePiece(GameRace race, string sprite, Vector2 offsetPosition, GameTile[] gameTiles, int i)
        {
            Drawable draw = new Drawable(GameEngine._sprites[sprite], gameTiles[i].Position + offsetPosition, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale));
            gamePieces[race].Add(new GamePiece(race, offsetPosition, gameTiles[i], draw));
            GameEngine.drawables.Add(draw);
        }
    }
}
