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
        public static float gameWidth = 1000;
        public static float gameHeight = 1000;
        public static float scaleWidth, scaleHeight;
        public MediaPlayer player;

        private bool _playing;

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

        /// <summary>
        /// Create hover effect for player
        /// </summary>
        private void GameCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            ClickedObject.Text = Input.MouseMoved(e, GameCanvas);
        }


        private void GameCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ClickedObject.Text = Input.MouseClicked(e, GameCanvas);

            DiceRoll.Text = Dice.DiceSave.ToString();
            Xmouseclick.Text = "Click X cord: " + (int)e.GetCurrentPoint(GameCanvas).Position.X;
            Ymouseclick.Text = "Click Y cord: " + (int)e.GetCurrentPoint(GameCanvas).Position.Y;

        }


        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            GameEngine.GameModeSwitch(GameCanvas);

            if (GameEngine.currentGameState == GameState.InMenu)
            {
                PauseMenu.Visibility = Visibility.Visible;
            }
            else
            {
                PauseMenu.Visibility = Visibility.Collapsed;
            }

        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void NewGame_ClickBtn(object sender, RoutedEventArgs e)
        {
            var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
            {
                GameEngine.currentGameState = GameState.PlayerPlaying;
            });
            StartMenu.Visibility = Visibility.Collapsed;
            GameEngine.StartGame(GameRace.Red);
        }

        private void MainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
            {
                GameEngine.currentGameState = GameState.InMenu;
            });
            PauseMenu.Visibility = Visibility.Collapsed;
            StartMenu.Visibility = Visibility.Visible;
        }

        public async void PlayBGM(object sender, RoutedEventArgs e)
        {
            Sound.PlayBGMusic();
        }

        public void RollDice(object sender, RoutedEventArgs e)
        {
            if (GameEngine.currentGameState == GameState.PlayerPlaying)
            {
                int number = Dice.RollDice();
                DiceRoll.Text = number.ToString();
                Dice.DiceSave = number; // saves what the dice show
            }
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