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
            _sprites = await FileHandeler.LoadSprites(sender, "Images");
            //menuBackGroundImage = _sprites["menubg"];
            //backgroundImage = _sprites["background"];
            //blackholeImage = _sprites["blackhole"];
            //greenBaseImg = _sprites["greenBase"];
            //blueBaseImg = _sprites["blueBase"];
            //redBaseImg = _sprites["redBase"];
            //yellowBaseImg = _sprites["yellowBase"];
            //whiteTileImg = _sprites["whiteTile"];
            //yellowTileImg = _sprites["yellowTile"];
            //blueTileImg = _sprites["blueTile"];
            //greenTileImg = _sprites["greenTile"];
            //redTileImg = _sprites["redTile"];
            drawables.Add(new Drawable(_sprites["background"], Vector2.Zero, 1, (bitmap, _) => Scaler.Fill(bitmap), true));
            CreateMap();
        }

        /// <summary>
        /// Calculate positions and draw tiles
        /// </summary>
        public static void CreateMap()
        {
            float width = (float)MainPage.GameWidth;
            float height = (float)MainPage.GameHeight;
            float baseLocation = 800;
            const int gameTileCount = 12 * 4;
            const int homeLocation = gameTileCount / 8;
            float distance = 900;
            float tileSize = 1;

            drawables.Add(new Drawable(_sprites["blackhole"], new Vector2(0, 0), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
            drawables.Add(new Drawable(_sprites["redBase"], new Vector2(baseLocation, baseLocation), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
            drawables.Add(new Drawable(_sprites["greenBase"], new Vector2(baseLocation, -baseLocation), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
            drawables.Add(new Drawable(_sprites["blueBase"], new Vector2(-baseLocation, baseLocation), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
            drawables.Add(new Drawable(_sprites["yellowBase"], new Vector2(-baseLocation, -baseLocation), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));

            float angle = 360.0f / gameTileCount * MathF.PI / 180.0f;


            for (int i = 0; i < gameTileCount; i++)
            {
                Vector2 tilePosition = new Vector2(MathF.Sin(angle * i) * distance, MathF.Cos(angle * i) * distance);
                CanvasBitmap tile;
                switch (i)
                {
                    case (homeLocation):
                        tile = _sprites["redTile"];
                        CreateHomeTiles(distance, tileSize, angle, i, tilePosition, tile);
                        break;

                    case (homeLocation * 3):
                        tile = _sprites["greenTile"];
                        CreateHomeTiles(distance, tileSize, angle, i, tilePosition, tile);
                        break;

                    case (homeLocation * 5):
                        tile = _sprites["yellowTile"];
                        CreateHomeTiles(distance, tileSize, angle, i, tilePosition, tile);
                        break;

                    case (homeLocation * 7):
                        tile = _sprites["blueTile"];
                        CreateHomeTiles(distance, tileSize, angle, i, tilePosition, tile);
                        break;

                    default:
                        drawables.Add(new Drawable(_sprites["whiteTile"], tilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
                        break;
                }
            }
        }

        private static void CreateHomeTiles(float distance, float tileSize, float angle, int i, Vector2 tilePosition, CanvasBitmap tile)
        {
            drawables.Add(new Drawable(tile, tilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
            for (int j = 1; j < 5; j++)
            {
                Vector2 homeTilePosition = new Vector2(MathF.Sin(angle * (i + j)) * (distance - (distance / 6) * (j)), MathF.Cos(angle * (i + j)) * (distance - (distance / 6) * (j)));
            drawables.Add(new Drawable(tile, homeTilePosition, tileSize, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
        }
    }

    //public void LoadAssets()
    //{
    //}
}
}