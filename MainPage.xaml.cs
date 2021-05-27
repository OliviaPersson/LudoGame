using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Media.Imaging;
using System.Numerics;
using LudoGame.Classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LudoGame
{
    public partial class MainPage : Page
    {
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float GameWidth = 1000;
        public static float GameHeight = 1000;
        public static float scaleWidth, scaleHeight;
        MediaPlayer player;
        bool playing;

        public MainPage()
        {

            this.InitializeComponent();
            player = new MediaPlayer();
            Window.Current.SizeChanged += Current_SizeChanged;
            Scaler.SetScale();
            // Init all GameStates
            GameEngine.InitializeGameEngine(GameCanvas);
            GameEngine.GameStateInit();
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Scaler.SetScale();
            GameEngine.OnSizeChanged();
        }

        private void GameCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Xmouseclick.Text = "Click X cord: " + e.GetCurrentPoint(GameCanvas).Position.X;
            Ymouseclick.Text = "Click Y cord: " + e.GetCurrentPoint(GameCanvas).Position.Y;
            gamestate.Text = "State: " + GameEngine.CurrentGameState.ToString();

            /*
            Point position = e.GetCurrentPoint(GameCanvas).Position;

            GamePiece selectedPiece = GamePiece.SelectPiece(position);
            */
          
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameEngine.CurrentGameState == 1 || GameEngine.CurrentGameState == 2)//If the game is paused or is playing
            {
                if (GameEngine.CurrentGameState == 1)
                {
                    var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
                    {
                        GameEngine.CurrentGameState = 2; // Pause
                    });
                    PauseBtn.Visibility = Visibility.Collapsed;
                    PauseMenu.Visibility = Visibility.Visible;
                }
                if (GameEngine.CurrentGameState == 2)
                {
                    var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
                    {
                        GameEngine.CurrentGameState = 1; //Play
                    });
                    PauseMenu.Visibility = Visibility.Collapsed;
                    PauseBtn.Visibility = Visibility.Visible;
                }
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
                    GameEngine.CurrentGameState = 1;
                });
                PauseBtn.Visibility = Visibility.Visible;
            StartMenu.Visibility = Visibility.Collapsed;
            
        }

        private void MainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
            {
                GameEngine.CurrentGameState = 0;
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
        }

        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e){}
    }
}
