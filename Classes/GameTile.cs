using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LudoGame.Classes
{
    public class GameTile
    {

        public GameRace raceHome;
        public Drawable drawable;

        public GameTile nextTile;
        public GameTile nextHomeTile;

        public GameTile previousTile
        {
            get { return _previousTile; }
            set
            {
                if (value != null)
                {
                    _previousTile = value;

                    if (raceHome != 0 && _previousTile.raceHome == raceHome)
                    {
                        _previousTile.nextHomeTile = this;
                    }
                    else
                    {
                        _previousTile.nextTile = this;
                    }
                }
            }
        }

        private GameTile _previousTile;

        public Vector2 Position
        {
            get
            {
                return drawable.Position;
            }
        }

        public GameTile(Drawable drawable, GameRace raceHome, GameTile previousTile)
        {
            this.drawable = drawable;
            this.raceHome = raceHome;
            this.previousTile = previousTile;
        }

        public GameTile GetNextTile(GameRace gameRace)
        {
            if (gameRace == raceHome && nextHomeTile != null)
            {
                return nextHomeTile;
            }
            else
            {
                return nextTile;
            }
        }

        /// <summary>
        /// Creates all of the game tiles
        /// </summary>
        /// <param name="sprites">a reference to the GameEngine sprites</param>
        /// <returns></returns>
        public static GameTile[] CreateGameTiles(Dictionary<string, CanvasBitmap> sprites) // this should be cleaned up
        {
            const int gameTileCount = 12 * 4;
            const int homeLocation = gameTileCount / 8;
            const int redHome = 1;
            const int greenHome = homeLocation * 2 + 1;
            const int yellowHome = homeLocation * 4;
            const int blueHome = homeLocation * 6;

            float baseLocation = 800;
            float distance = 900;
            float tileSize = 1;
            float angle = 360.0f / gameTileCount * MathF.PI / 180.0f;
            float angleOffset = 45 * MathF.PI / 180;

            List<GameTile> gameTiles = new List<GameTile>();
            GameTile previousTile = null;
            GameTile[] homeTiles = null;
            GameTile blackhole = CreateTile(sprites["blackhole"], Vector2.Zero, 0.3f, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), GameRace.None, null);
            for (int i = 0; i < gameTileCount; i++)
            {
                Vector2 tilePosition = new Vector2(MathF.Sin(angle * i + angleOffset) * distance, MathF.Cos(angle * i + angleOffset) * distance);
                switch (i)
                {
                    case redHome:
                        homeTiles = CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, sprites["redTile"], (GameRace)1, previousTile, blackhole);
                        gameTiles.AddRange(homeTiles);
                        previousTile = homeTiles[0];

                        break;

                    case greenHome:
                        homeTiles = CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, sprites["greenTile"], (GameRace)2, previousTile, blackhole);
                        gameTiles.AddRange(homeTiles);
                        previousTile = homeTiles[0];
                        break;

                    case yellowHome:
                        homeTiles = CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, sprites["yellowTile"], (GameRace)3, previousTile, blackhole);
                        gameTiles.AddRange(homeTiles);
                        previousTile = homeTiles[0];
                        break;

                    case blueHome:
                        homeTiles = CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, sprites["blueTile"], (GameRace)4, previousTile, blackhole);
                        gameTiles.AddRange(homeTiles);
                        previousTile = homeTiles[0];
                        break;

                    default:
                        previousTile = CreateTile(sprites["whiteTile"], tilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), 0, previousTile);
                        gameTiles.Add(previousTile);
                        break;
                }
            }

            gameTiles[0].previousTile = gameTiles[gameTiles.Count - 1];

            CreateBaseTile(sprites["redBase"], gameTiles, new Vector2(baseLocation, baseLocation), (GameRace)1, redHome);
            CreateBaseTile(sprites["greenBase"], gameTiles, new Vector2(baseLocation, -baseLocation), (GameRace)2, greenHome + 4);
            CreateBaseTile(sprites["yellowBase"], gameTiles, new Vector2(-baseLocation, -baseLocation), (GameRace)3, yellowHome + 8);
            CreateBaseTile(sprites["blueBase"], gameTiles, new Vector2(-baseLocation, baseLocation), (GameRace)4, blueHome + 12);

            gameTiles.Add(blackhole);

            return gameTiles.ToArray();
        }

        /// <summary>
        /// Creates the base tile
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="gameTiles"></param>
        /// <param name="baseLocation"></param>
        /// <param name="race"></param>
        /// <param name="homeTileIndex"></param>
        private static void CreateBaseTile(CanvasBitmap sprite, List<GameTile> gameTiles, Vector2 baseLocation, GameRace race, int homeTileIndex)
        {
            GameTile baseTile = CreateTile(sprite, baseLocation, 0.15f, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), race, null);
            baseTile.nextTile = gameTiles[homeTileIndex + 5];

            gameTiles.Add(baseTile);
        }


        private static GameTile[] CreateHomeTiles(float distance, float tileSize, float angle, int i, Vector2 tilePosition, float angleOffset, CanvasBitmap sprite, GameRace gameRace, GameTile previousTile, GameTile gameTile)
        {
            List<GameTile> homeTiles = new List<GameTile>();
            GameTile previousHomeTile = CreateTile(sprite, tilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), gameRace, previousTile);
            homeTiles.Add(previousHomeTile);
            for (int j = 1; j < 5; j++)
            {
                Vector2 homeTilePosition = new Vector2(MathF.Sin(angle * (i + j) + angleOffset) * (distance - (distance / 6) * (j)), MathF.Cos(angle * (i + j) + angleOffset) * (distance - (distance / 6) * (j)));
                previousHomeTile = CreateTile(sprite, homeTilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), gameRace, previousHomeTile);
                homeTiles.Add(previousHomeTile);
            }
            homeTiles[homeTiles.Count - 1].nextTile = gameTile;
            return homeTiles.ToArray();
        }

        private static GameTile CreateTile(CanvasBitmap image, Vector2 position, float imageScale, Drawable.Scale scalerMethood, GameRace gameRace, GameTile previousTile)
        {
            Drawable newTile = new Drawable(image, position, imageScale, scalerMethood);
            GameEngine.drawables.Add(newTile);
            return new GameTile(newTile, gameRace, previousTile);
        }
    }
}