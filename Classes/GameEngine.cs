using LudoGame.Classes;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Media.Playback;
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

        public static void InitializeGameEngine(CanvasAnimatedControl canvas)
        {
            _gameCanvas = canvas;
            _gameCanvas.CreateResources += (sender, _) => CreateResources(sender);
            _gameCanvas.Draw += (sender, drawArgs) => Draw(sender, drawArgs);
            currentGameState = GameState.PlayerPlaying;
        }

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


        public static void GameModeSwitch(CanvasAnimatedControl canvas)
        {
            if (currentGameState == GameState.PlayerPlaying || currentGameState == GameState.AIPlaying)
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

        public static void OnSizeChanged()
        {
            foreach (var item in drawables)
            {
                item.CalculateActualPosition();
            }
        }
        ///<summary> method <c> ClickHitDetection </c>
        ///checks if the player clicks on a tile or a piece and returns what is clicked 
        ///<permission> Public </permission>
        ///<paramref name="mousePosition"> a Vector2 that has the cordinates for were the mouse is when clicked </paramref>
        ///<summary>
        public static object ClickHitDetection(Vector2 mousePosition)
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

        public static void Update()
        {
            float tileSpeed = 300;
            if (currentGameState != GameState.InMenu)
            {
                bool aPieceIsMoving = false;

                if (gamePieces[15] != null)
                {
                    foreach (GamePiece piece in gamePieces)
                    {
                        if (piece.tile != piece.moveToTile && piece.moveToTile != null)
                        {
                            if (Vector2Math.Magnitude(piece.tile.GetNextTile(piece.race).Position - piece.Position) < 5)
                            {
                                piece.Position = piece.tile.GetNextTile(piece.race).Position;
                                if (piece.tile != piece.moveToTile)
                                {
                                    piece.tile = piece.tile.GetNextTile(piece.race);
                                }
                            }
                            else
                            {
                                piece.Position += Vector2Math.Normalized(piece.tile.GetNextTile(piece.race).Position - piece.Position) * tileSpeed * (float)_gameCanvas.TargetElapsedTime.TotalSeconds;
                                aPieceIsMoving = true;
                            }
                            break;
                        }
                    }

                    if (!aPieceIsMoving)
                    {

                        Turn.CheckTurn();
                    }
                }
            }
        }



        public static void Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            sender.TargetElapsedTime = TimeSpan.FromMilliseconds(1000d / 30);

            Win2DDrawingHandler.Draw(args, drawables.ToArray());

            // remove before shipping
            //Win2DDrawingHandler.DrawGameTilesDebugLines(sender, args, _gameTiles);
        }

        ///<summary> method <c> CreateResources </c>
        ///Loads assets
        ///<permission> Public </permission>
        ///<summary>
        public static async void CreateResources(CanvasAnimatedControl sender)
        {
            Sprites = new Dictionary<string, CanvasBitmap>();

            await LoadSpriteFolder(sender, "Images");
            await LoadSpriteFolder(sender, "Tiles");
            await LoadSpriteFolder(sender, "Pieces");

            drawables.Add(new Drawable(Sprites["background"], Vector2.Zero, 1, (bitmap, _) => Scaler.Fill(bitmap)));
            drawables.Add(new Drawable(Sprites["blackhole"], Vector2.Zero, 1, (bitmap, scale) => Scaler.ImgUniform(bitmap, scale))); // , _sprites["blackholehighlighteffect"]

            _sounds = await FileHandeler.LoadSounds("Sounds");

            Sound.backgroundMusic = _sounds["backgroundMusic"];
            Sound.backgroundMusic.IsLoopingEnabled = true;
            Sound.backgroundMusic.AutoPlay = false;
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