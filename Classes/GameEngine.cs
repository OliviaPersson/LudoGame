using LudoGame.Classes;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace LudoGame.Classes
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
        public static GameState currentGameState = 0;
        public static Player player;
        public static AIPlayer[] aIPlayers = new AIPlayer[3];
        public static Player[] players;
        public static GamePiece[] gamePieces = new GamePiece[16];
        public static bool aPieceIsMoving;
        public static CanvasAnimatedControl GameCanvas { get { return _gameCanvas; } }
        public static GameTile[] GameTiles { get { return _gameTiles; } }
        //public static Player[] players;

        private static Dictionary<string, MediaPlayer> _sounds;
        private static Dictionary<string, CanvasBitmap> _sprites;
        private static CanvasAnimatedControl _gameCanvas;
        private static GameTile[] _gameTiles;

        private static Input _input;
        private static string _fileloction;
        private static Cards[] _cards;
        private static Wormhole _wormhole;
        private static GameState _saveCurrentState;

        public static DispatcherTimer timer;

        public static Dictionary<string, CanvasBitmap> Sprites { get => _sprites; set => _sprites = value; }

        /// <summary>
        /// Gets a reference to the canvas that is used to play the game,
        /// sets up the events used in game engine
        /// </summary>
        public static void InitializeGameEngine(CanvasAnimatedControl canvas)
        {
            _gameCanvas = canvas;
            _gameCanvas.CreateResources += (sender, _) => CreateResources(sender);
            _gameCanvas.Draw += (sender, drawArgs) => Draw(sender, drawArgs);
            currentGameState = GameState.PlayerPlaying;
        }

        /// <summary>
        /// Sets up the playing field and the players
        /// </summary>
        /// <param name="playerRace">The race of the player</param>
        public static void StartGame(GameRace playerRace)
        {
            _gameTiles = GameTile.CreateGameTiles(Sprites);
            //Changed so that all players is in the same player array
            players = new Player[] {
                new Player((GameRace)1, _sprites["redGamePiece"], 50, _gameTiles),
                new Player((GameRace)2, _sprites["greenGamePiece"], 50, _gameTiles),
                new Player((GameRace)3, _sprites["yellowGamePiece"], 50, _gameTiles),
                new Player((GameRace)4, _sprites["blueGamePiece"], 50, _gameTiles)
            };

            player = players[(int)playerRace - 1];

            // assigns the rest of the player races to the 3 ai players
            int aiAssigned = 0;
            for (int i = 1; i < 5; i++)
            {
                if (player.race != (GameRace)i)
                {
                    aIPlayers[aiAssigned] = new AIPlayer(players[i - 1]);
                    aiAssigned++;
                }
            }

            int gamePieceAssigned = 0;
            foreach (Player player in players)
            {
                foreach (GamePiece gamePiece in player.GamePieces)
                {
                    gamePieces[gamePieceAssigned] = gamePiece;
                    gamePieceAssigned++;
                }
            }
            Play();
        }

        /// <summary>
        /// Used pause the game when the pause menu is brought up
        /// </summary>
        /// <param name="canvas"></param>
        public static void GameModeSwitch(CanvasAnimatedControl canvas)
        {
            if (currentGameState != GameState.InMenu)
            {
                _saveCurrentState = currentGameState;
                var action = canvas.RunOnGameLoopThreadAsync(() =>
                {
                    currentGameState = GameState.InMenu;
                });
            }
            else
            {
                var action = canvas.RunOnGameLoopThreadAsync(() =>
                {
                    currentGameState = _saveCurrentState; //Play
                });
                //UI.FinishGamePiece(gamePieces[13]);
            }
        }


        public static void Pause()
        {
            _gameCanvas.Update -= (s, a) => Update();
        }

        public static void Play()
        {
            _gameCanvas.Update += (s, a) => Update();
        }

        /// <summary>
        /// Recalculates the positions of every drawable on window size change
        /// </summary>
        public static void OnSizeChanged()
        {
            Drawable[] d = drawables.ToArray();
            foreach (var item in d)
            {
                item.CalculateActualPosition();
            }
        }

        /// <summary>
        /// Checks if the position overlaps with an interacteble object
        /// <summary>
        /// <paramref name="mousePosition"> a Vector2 that has the cordinates for were the mouse is when clicked </paramref>
        /// <returns>Any interactable object that was hit</returns>

        public static object HitDetection(Vector2 mousePosition)
        {
            if (player != null)
            {
                for (int i = 0; i < player.GamePieces.Length; i++)
                {
                    Vector2 distance = mousePosition - player.GamePieces[i].drawable.ActualPosition;

                    if (distance.X >= 0 &&
                        distance.Y >= 0 &&
                        distance.X <= player.GamePieces[i].drawable.ScaledSize.X &&
                        distance.Y <= player.GamePieces[i].drawable.ScaledSize.Y)
                    {
                        return player.GamePieces[i];
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

        /// <summary>
        /// Anything that needs to be updated at a fixed rate is called from here,
        /// this is hooked to the canvas update event
        /// </summary>
        public static void Update()
        {
            float tileSpeed = 300;
            if (currentGameState != GameState.InMenu)
            {
                if (gamePieces.Length > 0)
                {
                    if (!GamePiece.TryMovingPiece(tileSpeed))
                    {
                        Turn.CheckTurn();
                    }
                }
            }
        }

        /// <summary>
        /// Anything that needs to be done before drawing a new image on the canvas is called from here
        /// </summary>
        /// <param name="sender">The canvas that triggered the draw event</param>
        /// <param name="args"></param>
        public static void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            //sender.TargetElapsedTime = TimeSpan.FromMilliseconds(1000d / 30);

            Win2DDrawingHandler.Draw(args, drawables.ToArray());

            // DEBUGING remove before shipping
            Win2DDrawingHandler.DrawGameTilesDebugLines(sender, args, _gameTiles);
        }

        /// <summary>
        /// Loads assets
        /// <summary>
        public static async void CreateResources(CanvasAnimatedControl sender)
        {
            Sprites = new Dictionary<string, CanvasBitmap>();

            await LoadSpriteFolder(sender, "Images");
            await LoadSpriteFolder(sender, "Tiles");
            await LoadSpriteFolder(sender, "Pieces");

            //drawables.Add(new Drawable(Sprites["background"], Vector2.Zero, 1, (bitmap, _) => Scaler.Fill(bitmap)));

            _sounds = await FileHandeler.LoadSounds("Sounds");

            Sound.backgroundMusic = _sounds["backgroundMusic"];
            Sound.backgroundMusic.IsLoopingEnabled = true;
            Sound.backgroundMusic.AutoPlay = false;
        }

        /// <summary>
        /// Loads the folder and ads them to the sprites with a nameID as a key
        /// to be used in a drawable.
        /// </summary>
        /// <param name="sender">The canvas where the sprites are loaded to</param>
        /// <param name="folder">The folder to load sprites from</param>
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