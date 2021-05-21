using LudoGame.Classes;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            drawables.Add(new Drawable(_sprites["background"], Vector2.Zero, 1, (bitmap, _) => Scaler.Fill(bitmap), true));
            drawables.Add(new Drawable(_sprites["blackhole"], new Vector2(0, 0), 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale)));

            //// Menu
            //menuBackGroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/menubg.png"));

            //// Playing
            //backgroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/bg.png"));
            //blackholeImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/blackhole.png"));
        }
        //public void LoadAssets()
        //{

        //}
    }
}
