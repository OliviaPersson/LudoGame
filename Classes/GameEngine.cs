using LudoGame.Classes;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace LudoGame
{
    public static class GameEngine
    {
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
            _sprites = await FileHandeler.LoadImages(sender, "Images");
            drawables.Add(new Drawable(_sprites["background"], Vector2.Zero, 1, (bitmap, _) => Scaler.Fill(bitmap), true));

            foreach (var item in await FileHandeler.LoadImages(sender, "Tiles"))
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
            _gameTiles = CreateMap();
        }

        /// <summary>
        /// Calculate positions and draw tiles
        /// </summary>
        private static GameTile[] CreateMap()
        {
            List<GameTile> gameTiles = new List<GameTile>();

            float width = (float)MainPage.GameWidth;
            float height = (float)MainPage.GameHeight;
            float baseLocation = 800;
            const int gameTileCount = 12 * 4;
            const int homeLocation = gameTileCount / 8;
            float distance = 900;
            float tileSize = 1;

            drawables.Add(new Drawable(_sprites["blackhole"], Vector2.Zero, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
            gameTiles.Add(CreateTile(_sprites["redBase"], new Vector2(baseLocation, baseLocation), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), "redBase"));
            gameTiles.Add(CreateTile(_sprites["greenBase"], new Vector2(baseLocation, -baseLocation), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), "greenBase"));
            gameTiles.Add(CreateTile(_sprites["blueBase"], new Vector2(-baseLocation, baseLocation), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), "blueBase"));
            gameTiles.Add(CreateTile(_sprites["yellowBase"], new Vector2(-baseLocation, -baseLocation), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), "yellowBase"));

            float angle = 360.0f / gameTileCount * MathF.PI / 180.0f;
            float angleOffset = 45 * MathF.PI / 180;


            for (int i = 0; i < gameTileCount; i++)
            {
                Vector2 tilePosition = new Vector2(MathF.Sin(angle * i + angleOffset) * distance, MathF.Cos(angle * i + angleOffset) * distance);
                CanvasBitmap tile;
                switch (i)
                {
                    case (0):
                        tile = _sprites["redTile"];
                        gameTiles.AddRange(CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, "redTile"));
                        break;

                    case (homeLocation * 2):
                        tile = _sprites["greenTile"];
                        gameTiles.AddRange(CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, "greenTile"));
                        break;

                    case (homeLocation * 4):
                        tile = _sprites["yellowTile"];
                        gameTiles.AddRange(CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, "yellowTile"));
                        break;

                    case (homeLocation * 6):
                        tile = _sprites["blueTile"];
                        gameTiles.AddRange(CreateHomeTiles(distance, tileSize, angle, i, tilePosition, angleOffset, "blueTile"));
                        break;

                    default:
                        gameTiles.Add(CreateTile(_sprites["whiteTile"], tilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), "whiteTile"));
                        break;
                }
            }

            return gameTiles.ToArray();
        }

        private static GameTile[] CreateHomeTiles(float distance, float tileSize, float angle, int i, Vector2 tilePosition, float angleOffset, string spriteID)
        {
            List<GameTile> homeTiles = new List<GameTile>();

            homeTiles.Add(CreateTile(_sprites[spriteID], tilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), spriteID + 0));
            for (int j = 1; j < 5; j++)
            {
                Vector2 homeTilePosition = new Vector2(MathF.Sin(angle * (i + j) + angleOffset) * (distance - (distance / 6) * (j)), MathF.Cos(angle * (i + j) + angleOffset) * (distance - (distance / 6) * (j)));
                homeTiles.Add(CreateTile(_sprites[spriteID], homeTilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale), spriteID + j));
            }

            return homeTiles.ToArray();
        }

        private static GameTile CreateTile(CanvasBitmap image, Vector2 position, float imageScale, Drawable.Scale scalerMethood, string nameID)
        {
            Drawable newTile = new Drawable(image, position, imageScale, scalerMethood);
            drawables.Add(newTile);
            return new GameTile(nameID, newTile);
        }

        //public void LoadAssets()
        //{
        //}
    }
}