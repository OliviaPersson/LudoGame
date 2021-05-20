using LudoGame.Classes;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    public class GameEngine
    {
        public static CanvasBitmap backgroundImage, blackholeImage, menuBackGroundImage, greenBaseImg, blueBaseImg, redBaseImg, yellowBaseImg, whiteTileImg, yellowTileImg, blueTileImg, greenTileImg, redTileImg;
        private Dictionary<string, CanvasBitmap> _sprites;
        //private Sound[] _sounds;
        private GameTile[] _gameTiles;
        private Player _player;
        private AIPlayer _aIPlayer;
        private InputReader _input;
        private string _fileloction;
        private Cards[] _cards;
        private Wormhole _wormhole;

        public static int CurrentGameState = 0;
        public List<GameState> GameStates = new List<GameState>();

        public void GameStateInit()
        {
            GameStates.Add(new GameState() { name = "Menu" });
            GameStates.Add(new GameState() { name = "Playing" });
            GameStates.Add(new GameState() { name = "Paused" });
            GameStates.Add(new GameState() { name = "GameOver" });
        }

        // Load Asset
        public async Task CreateResources(CanvasAnimatedControl sender)
        {
            _sprites = await FileHandeler.LoadSprites(sender, "Images");
            menuBackGroundImage = _sprites["menubg"];
            backgroundImage = _sprites["background"];
            blackholeImage = _sprites["blackhole"];
            greenBaseImg = _sprites["greenBase"];
            blueBaseImg = _sprites["blueBase"];
            redBaseImg = _sprites["redBase"];
            yellowBaseImg = _sprites["yellowBase"];
            whiteTileImg = _sprites["whiteTile"];
            yellowTileImg = _sprites["yellowTile"];
            blueTileImg = _sprites["blueTile"];
            greenTileImg = _sprites["greenTile"];
            redTileImg = _sprites["redTile"];

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
