using LudoGame.Classes;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LudoGame
{
    public partial class MainPage : Page
    {
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float GameWidth = 1000;
        public static float GameHeight = 1000;
        public static float scaleWidth, scaleHeight;
        private MediaPlayer player;
        private bool playing;
        private GameState saveCurrentState;
        private int DiceSave;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += Global_KeyDown;
            player = new MediaPlayer();
            Window.Current.SizeChanged += Current_SizeChanged;
            Scaler.SetScale();
            GameEngine.InitializeGameEngine(GameCanvas);

            Size minSize = new Size(1420, 800);
            if (Window.Current.Bounds.Y < minSize.Height || Window.Current.Bounds.X < minSize.Width)
            {
                ApplicationView.GetForCurrentView().TryResizeView(new Size(1420, 800));
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Scaler.SetScale();
            GameEngine.OnSizeChanged();
        }

        private void GameCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            object selectedGamePiece = null;
            Xmouseclick.Text = "Click X cord: " + (int)e.GetCurrentPoint(GameCanvas).Position.X;
            Ymouseclick.Text = "Click Y cord: " + (int)e.GetCurrentPoint(GameCanvas).Position.Y;
            //gamestate.Text = "State: " + GameEngine.CurrentGameState.ToString();

            if (GameEngine.CurrentGameState == GameState.PlayerPlaying)
            {
                object returned = GameEngine.ClickHitDetection(new Vector2((float)e.GetCurrentPoint(GameCanvas).Position.X, (float)e.GetCurrentPoint(GameCanvas).Position.Y));

                if(returned is GamePiece)
                {
                    selectedGamePiece = returned;
                    GamePiece.MoveToGameTile(DiceSave, (GamePiece)selectedGamePiece);//Calls to move func in Gamepice
                    DiceSave = 0;
                }
                else if(returned is GameTile && selectedGamePiece != null)
                {
                    GamePiece.MovePiece((GamePiece)selectedGamePiece, (GameTile)returned);
                    selectedGamePiece = null;
                }

                if (returned is GamePiece) ClickedObject.Text = "GamePiece";
                else if (returned is GameTile) ClickedObject.Text = "GameTile";
                else ClickedObject.Text = "Null";
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            GameState state0 = (GameState)0;// Menu
            GameState state1 = (GameState)1;//PlayerPlaying
            GameState state2 = (GameState)2;//AIPlaying

            if (GameEngine.CurrentGameState == state1 || GameEngine.CurrentGameState == state2)
            {
                saveCurrentState = GameEngine.CurrentGameState;
                var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
                {
                    GameEngine.CurrentGameState = state0; // Pause
                });
                PauseMenu.Visibility = Visibility.Visible;
            }
            else if (GameEngine.CurrentGameState == state0)
            {
                var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
                {
                    GameEngine.CurrentGameState = saveCurrentState; //Play
                });
                PauseMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void NewGame_ClickBtn(object sender, RoutedEventArgs e)
        {
            GameState state1 = (GameState)1;//PlayerPlaying

            var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
            {
                GameEngine.CurrentGameState = state1;
            });
            StartMenu.Visibility = Visibility.Collapsed;

            GameEngine.StartGame();
        }

        private void MainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
            {
                GameEngine.CurrentGameState = GameState.PlayerPlaying;
            });
            PauseMenu.Visibility = Visibility.Collapsed;
            StartMenu.Visibility = Visibility.Visible;
        }

        public async void PlayBGM(object sender, RoutedEventArgs e)
        {
            //Sound.PlayBGMSound();
            //https://pixabay.com/music/search/genre/ambient/
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("pixabay-1-min-piano_arp-4222.mp3");
            player.AutoPlay = false;
            player.Source = MediaSource.CreateFromStorageFile(file);
            if (playing)
            {
                player.Source = null;
                playing = false;
            }
            else
            {
                player.Play();
                playing = true;
            }
        }

        public void RollDice(object sender, RoutedEventArgs e)
        {
            int number = Dice.randomNum();
            DiceRoll.Text = number.ToString();
            DiceSave = number; // saves what the dice show
        }

        private void OptionsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OptionsMenu.Visibility == Visibility.Collapsed)
            {
                OptionsMenu.Visibility = Visibility.Visible;
            }
            else
            {
                OptionsMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void Global_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Escape)
            {
                PauseButton_Click(sender, new RoutedEventArgs());
            }
        }

        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }
    }
}