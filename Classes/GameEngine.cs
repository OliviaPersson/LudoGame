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
    public enum GameState
    {
        InMenu,
        PlayerPlaying,
        AIPlaying
    }
    public static class GameEngine
    {
        public static List<Drawable> drawables = new List<Drawable>();
        public static GameState CurrentGameState = 0;

        private static Dictionary<string, CanvasBitmap> _sprites;
        private static CanvasAnimatedControl _gameCanvas;
        private static GameTile[] _gameTiles;
        private static Player _player;
        //private static Sound[] _sounds;
        private static AIPlayer[] _aIPlayers;
        private static InputReader _input;
        private static string _fileloction;
        private static Cards[] _cards;
        private static Wormhole _wormhole;

        public static Dictionary<string, CanvasBitmap> Sprites { get => _sprites; set => _sprites = value; }

        public static void InitializeGameEngine(CanvasAnimatedControl canvas)
        {
            _gameCanvas = canvas;
            _gameCanvas.CreateResources += (sender, _) => CreateResources(sender);
            _gameCanvas.Draw += (sender, drawArgs) => Draw(sender, drawArgs);
            CurrentGameState = GameState.PlayerPlaying;
        }

        public static void StartGame()
        {
            _gameTiles = GameTile.CreateGameTiles(Sprites);
            //GamePiece.InitializeGamePieces(_gameTiles);
            _player = new Player((GameRace)1, _sprites["redGamePiece"], 50, _gameTiles);
            _aIPlayers = new AIPlayer[] {
                new AIPlayer(new Player((GameRace)2, _sprites["greenGamePiece"], 50, _gameTiles)),
                new AIPlayer(new Player((GameRace)3, _sprites["yellowGamePiece"], 50, _gameTiles)),
                new AIPlayer(new Player((GameRace)4, _sprites["blueGamePiece"], 50, _gameTiles))
            };
            Play();
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

        public static object ClickHitDetection(Vector2 mousePosition)
        {
            if (_player != null)
            {
                for (int i = 0; i < _player.GamePieces.Length; i++)
                {
                    Vector2 distance = mousePosition - _player.GamePieces[i].drawable.ActualPosition;

                    if (distance.X >= 0 &&
                        distance.Y >= 0 &&
                        distance.X <= _player.GamePieces[i].drawable.ScaledSize.X &&
                        distance.Y <= _player.GamePieces[i].drawable.ScaledSize.Y)
                    {
                        return _player.GamePieces[i];
                    }
                }
            }

            if (_aIPlayers != null)
            {
                for (int j = 0; j < _aIPlayers.Length; j++)
                {
                    for (int i = 0; i < _aIPlayers[j].Player.GamePieces.Length; i++)
                    {
                        Vector2 distance = mousePosition - _aIPlayers[j].Player.GamePieces[i].drawable.ActualPosition;

                        if (distance.X >= 0 &&
                            distance.Y >= 0 &&
                            distance.X <= _aIPlayers[j].Player.GamePieces[i].drawable.ScaledSize.X &&
                            distance.Y <= _aIPlayers[j].Player.GamePieces[i].drawable.ScaledSize.Y)
                        {
                            return _aIPlayers[j].Player.GamePieces[i];
                        }
                    }
                }

            }

            if (_gameTiles != null)
            {
                for (int i = 0; i < _gameTiles.Length; i++)
                {
                    Vector2 distance = mousePosition - _gameTiles[i].drawable.ActualPosition;

                    if (distance.X >= 0 &&
                        distance.Y >= 0 &&
                        distance.X <= _gameTiles[i].drawable.ScaledSize.X &&
                        distance.Y <= _gameTiles[i].drawable.ScaledSize.Y)
                    {
                        return _gameTiles[i];
                    }
                }
            }

            return null;
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

        // Load Asset
        public static async void CreateResources(CanvasAnimatedControl sender)
        {
            Sprites = new Dictionary<string, CanvasBitmap>();

            await LoadSpriteFolder(sender, "Images");
            await LoadSpriteFolder(sender, "Tiles");
            await LoadSpriteFolder(sender, "Pieces");

            drawables.Add(new Drawable(Sprites["background"], Vector2.Zero, 1, (bitmap, _) => Scaler.Fill(bitmap)));
            drawables.Add(new Drawable(Sprites["blackhole"], Vector2.Zero, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));
        }



        private static async Task LoadSpriteFolder(CanvasAnimatedControl sender, string folder)
        {
            foreach (var item in await FileHandeler.LoadImages(sender, folder))
            {
                try
                {
                    Sprites.Add(item.Key, item.Value);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }

    //public void LoadAssets()
    //{
    //}
}
