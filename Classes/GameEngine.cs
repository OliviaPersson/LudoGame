using LudoGame.Classes;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas.Brushes;
using System.Threading.Tasks;

namespace LudoGame
{
    public enum GameRace
    {
        None,
        Red,
        Green,
        Yellow,
        Blue
    }

    public static class GameEngine
    {
        public static Dictionary<string, List<GamePiece>> gamePieces = new Dictionary<string, List<GamePiece>>();

        public static List<Drawable> drawables = new List<Drawable>();
        public static int CurrentGameState = 0;
        public static List<GameState> GameStates = new List<GameState>();

        private static CanvasAnimatedControl _gameCanvas;
        private static Dictionary<string, CanvasBitmap> _sprites;

        //private static Sound[] _sounds;
        private static GameTile[] _gameTiles;

        private static Player _player;
        private static AIPlayer _aIPlayer;
        private static InputReader _input;
        private static string _fileloction;
        private static Cards[] _cards;
        private static Wormhole _wormhole;

        public static void InitializeGameEngine(CanvasAnimatedControl canvas)
        {
            _gameCanvas = canvas;

            _gameCanvas.CreateResources += (sender, _) => CreateResources(sender);
            Play();
            _gameCanvas.Draw += (sender, drawArgs) => Draw(sender, drawArgs);
        }

        public static void Pause()
        {
            _gameCanvas.Update -= (s, a) => Update();
        }

        public static void Play()
        {
            _gameCanvas.Update += (s, a) => Update();
        }

        public static void OnSizeChanged()
        {
            foreach (var item in drawables)
            {
                item.CalculateActualPosition();
            }
        }

        public static void Update()
        {
        }

        public static void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            Win2DDrawingHandler.Draw(args, drawables.ToArray());

            // remove before shipping
            Win2DDrawingHandler.DrawGameTilesDebugLines(sender, args, _gameTiles);
        }

        public static void GameStateInit()
        {
            GameStates.Add(new GameState() { name = "Menu" });
            GameStates.Add(new GameState() { name = "Playing" });
            GameStates.Add(new GameState() { name = "Paused" });
            GameStates.Add(new GameState() { name = "GameOver" });
        }

        // Load Asset
        public static async void CreateResources(CanvasAnimatedControl sender)
        {
            _sprites = new Dictionary<string, CanvasBitmap>();

            await LoadSpriteFolder(sender, "Images");
            await LoadSpriteFolder(sender, "Tiles");
            await LoadSpriteFolder(sender, "Pieces");

            drawables.Add(new Drawable(_sprites["background"], Vector2.Zero, 1, (bitmap, _) => Scaler.Fill(bitmap)));
            drawables.Add(new Drawable(_sprites["blackhole"], Vector2.Zero, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
            _gameTiles = CreateGameTiles();
            InitializeGamePieces();
        }

        private static async Task LoadSpriteFolder(CanvasAnimatedControl sender, string folder)
        {
            foreach (var item in await FileHandeler.LoadImages(sender, folder))
            {
                try
                {
                    _sprites.Add(item.Key, item.Value);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Calculate positions and draw tiles
        /// </summary>
        private static GameTile[] CreateGameTiles()
        {
            const int gameTileCount = 12 * 4;
            const int homeLocation = gameTileCount / 8;
            const int redHome = 0;
            const int greenHome = homeLocation * 2;
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
            for (int i = 0; i < gameTileCount; i++)
            {
                Vector2 tilePosition = new Vector2(MathF.Sin(angle * i + angleOffset) * distance, MathF.Cos(angle * i + angleOffset) * distance);
                switch (i)
                {
                    case redHome:
                        homeTiles = CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, "redTile", (GameRace)1, previousTile);
                        gameTiles.AddRange(homeTiles);
                        previousTile = homeTiles[0];
                        break;

                    case greenHome:
                        homeTiles = CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, "greenTile", (GameRace)2, previousTile);
                        gameTiles.AddRange(homeTiles);
                        previousTile = homeTiles[0];
                        break;

                    case yellowHome:
                        homeTiles = CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, "yellowTile", (GameRace)3, previousTile);
                        gameTiles.AddRange(homeTiles);
                        previousTile = homeTiles[0];
                        break;

                    case blueHome:
                        homeTiles = CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, "blueTile", (GameRace)4, previousTile);
                        gameTiles.AddRange(homeTiles);
                        previousTile = homeTiles[0];
                        break;

                    default:
                        previousTile = CreateTile(_sprites["whiteTile"], tilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), 0, previousTile);
                        gameTiles.Add(previousTile);
                        break;
                }
            }

            gameTiles[0].previousTile = gameTiles[gameTiles.Count - 1];

            CreateBaseTile("redBase", gameTiles, new Vector2(baseLocation, baseLocation), (GameRace)1, redHome);
            CreateBaseTile("greenBase", gameTiles, new Vector2(baseLocation, -baseLocation), (GameRace)2, greenHome + 4);
            CreateBaseTile("yellowBase", gameTiles, new Vector2(-baseLocation, -baseLocation), (GameRace)3, yellowHome + 8);
            CreateBaseTile("blueBase", gameTiles, new Vector2(-baseLocation, baseLocation), (GameRace)4, blueHome + 12);

            return gameTiles.ToArray();
        }

        private static void CreateBaseTile(string spriteKey, List<GameTile> gameTiles, Vector2 baseLocation, GameRace race, int homeTileIndex)
        {
            GameTile baseTile = CreateTile(_sprites[spriteKey], baseLocation, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), race, null);
            baseTile.nextTile = gameTiles[homeTileIndex + 5];
            gameTiles.Add(baseTile);
        }

        private static GameTile[] CreateHomeTiles(float distance, float tileSize, float angle, int i, Vector2 tilePosition, float angleOffset, string spriteID, GameRace gameRace, GameTile previousTile)
        {
            List<GameTile> homeTiles = new List<GameTile>();
            GameTile previousHomeTile = CreateTile(_sprites[spriteID], tilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), gameRace, previousTile);
            homeTiles.Add(previousHomeTile);
            for (int j = 1; j < 5; j++)
            {
                Vector2 homeTilePosition = new Vector2(MathF.Sin(angle * (i + j) + angleOffset) * (distance - (distance / 6) * (j)), MathF.Cos(angle * (i + j) + angleOffset) * (distance - (distance / 6) * (j)));
                previousHomeTile = CreateTile(_sprites[spriteID], homeTilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), gameRace, previousHomeTile);
                homeTiles.Add(previousHomeTile);
            }

            return homeTiles.ToArray();
        }

        private static GameTile CreateTile(CanvasBitmap image, Vector2 position, float imageScale, Drawable.Scale scalerMethood, GameRace gameRace, GameTile previousTile)
        {
            Drawable newTile = new Drawable(image, position, imageScale, scalerMethood);
            drawables.Add(newTile);
            return new GameTile(newTile, gameRace, previousTile);
        }

        public static void InitializeGamePieces()
        {
            float baseLocation = 800;
            float offset = 50;

            gamePieces.Add("red", new List<GamePiece>());
            gamePieces.Add("blue", new List<GamePiece>());
            gamePieces.Add("yellow", new List<GamePiece>());
            gamePieces.Add("green", new List<GamePiece>());

            gamePieces["red"].Add(new GamePiece("red", "redGamePiece", new Vector2(baseLocation + offset, baseLocation + offset)));
            gamePieces["red"].Add(new GamePiece("red", "redGamePiece", new Vector2(baseLocation - offset, baseLocation - offset)));
            gamePieces["red"].Add(new GamePiece("red", "redGamePiece",  new Vector2(baseLocation + offset, baseLocation - offset)));
            gamePieces["red"].Add(new GamePiece("red", "redGamePiece", new Vector2(baseLocation - offset, baseLocation + offset)));

            gamePieces["blue"].Add(new GamePiece("blue", "blueGamePiece", new Vector2(-baseLocation + offset, baseLocation + offset)));
            gamePieces["blue"].Add(new GamePiece("blue", "blueGamePiece", new Vector2(-baseLocation - offset, baseLocation - offset)));
            gamePieces["blue"].Add(new GamePiece("blue", "blueGamePiece", new Vector2(-baseLocation + offset, baseLocation - offset)));
            gamePieces["blue"].Add(new GamePiece("blue", "blueGamePiece", new Vector2(-baseLocation - offset, baseLocation + offset)));

            gamePieces["yellow"].Add(new GamePiece("yellow", "yellowGamePiece", new Vector2(-baseLocation + offset, -baseLocation + offset)));
            gamePieces["yellow"].Add(new GamePiece("yellow", "yellowGamePiece", new Vector2(-baseLocation - offset, -baseLocation - offset)));
            gamePieces["yellow"].Add(new GamePiece("yellow", "yellowGamePiece", new Vector2(-baseLocation + offset, -baseLocation - offset)));
            gamePieces["yellow"].Add(new GamePiece("yellow", "yellowGamePiece", new Vector2(-baseLocation - offset, -baseLocation + offset)));

            gamePieces["green"].Add(new GamePiece("green", "greenGamePiece", new Vector2(baseLocation + offset, -baseLocation + offset)));
            gamePieces["green"].Add(new GamePiece("green", "greenGamePiece", new Vector2(baseLocation - offset, -baseLocation - offset)));
            gamePieces["green"].Add(new GamePiece("green", "greenGamePiece", new Vector2(baseLocation + offset, -baseLocation - offset)));
            gamePieces["green"].Add(new GamePiece("green", "greenGamePiece", new Vector2(baseLocation - offset, -baseLocation + offset)));

            foreach(string key in gamePieces.Keys)
            {
                for (int i = 0; i < gamePieces[key].Count; i++)
                {
                    drawables.Add(new Drawable(_sprites[gamePieces[key][i].spriteName], gamePieces[key][i].homePosition, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
                }
            }
        }
    }

    //public void LoadAssets()
    //{
    //}
}
