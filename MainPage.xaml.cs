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
using Windows.UI.Xaml.Shapes;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LudoGame
{
    public partial class MainPage : Page
    {
        public static Vector2 bounds;
        public static float gameWidth = 1000;
        public static float gameHeight = 1000;
        public static float scaleWidth, scaleHeight;

        public MainPage()
        {
            this.InitializeComponent();
            // the expected configuration on game start
            // to simplify editing in the xamal
            StartMenu.Visibility = Visibility.Visible;
            PauseMenu.Visibility = Visibility.Collapsed;

            Window.Current.CoreWindow.KeyDown += Global_KeyDown;
            GameCanvas.SizeChanged += (s, _) => RecalculateCanvasSizes();

            RecalculateCanvasSizes();
            GameEngine.InitializeGameEngine(GameCanvas);

            Rectangle[] rectangles = new Rectangle[] { 
                RedPiece1, RedPiece2, RedPiece3, RedPiece4, 
                GreenPiece1, GreenPiece2, GreenPiece3, GreenPiece4, 
                YellowPiece1, YellowPiece2, YellowPiece3, YellowPiece4, 
                BluePiece1, BluePiece2, BluePiece3, BluePiece4 };
            UI.Initialize(rectangles);
        }


        private void RecalculateCanvasSizes()
        {
            bounds = GameCanvas.ActualSize;
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
            if (GameEngine.currentGameState != GameState.InMenu)
            {
                PauseMenu.Visibility = Visibility.Visible;
            }
            else
            {
                PauseMenu.Visibility = Visibility.Collapsed;
            }

            GameEngine.GameModeSwitch(GameCanvas);
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void NewGame_ClickBtn(object sender, RoutedEventArgs e)
        {
            _ = GameCanvas.RunOnGameLoopThreadAsync(() =>
            {
                GameEngine.currentGameState = GameState.PlayerPlaying;
            });
            StartMenu.Visibility = Visibility.Collapsed;
            GameEngine.StartGame(GameRace.Red);
        }

        private void MainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            _ = GameCanvas.RunOnGameLoopThreadAsync(() =>
            {
                GameEngine.currentGameState = GameState.InMenu;
            });
            PauseMenu.Visibility = Visibility.Collapsed;
            StartMenu.Visibility = Visibility.Visible;
        }

        public void PlayBGM(object sender, RoutedEventArgs e)
        {
            Sound.PlayBGMusic();
        }

        public void RollDice(object sender, RoutedEventArgs e)
        {
            if (GameEngine.currentGameState == GameState.PlayerPlaying)
            {
                int number = Dice.RollDice();
                DiceRoll.Text = number.ToString();
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

        private void VolumeSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            Sound.SetVolume(e.NewValue / 100);
        }

        private void CheatRoll_Click(object sender, RoutedEventArgs e)
        {
            if (GameEngine.currentGameState == GameState.PlayerPlaying)
            {
                Int32.TryParse(CheatDice.Text, out int cheatRoll);
                int number = Dice.Cheat(cheatRoll);
                DiceRoll.Text = number.ToString();
            }
        }
    }
}