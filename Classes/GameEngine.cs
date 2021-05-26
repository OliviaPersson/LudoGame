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
            _gameTiles = GameTile.CreateGameTiles(_sprites);
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
