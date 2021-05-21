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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LudoGame
{
    public sealed partial class MainPage : Page
    {
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float GameWidth = 1000;
        public static float GameHeight = 1000;
        public static float scaleWidth, scaleHeight;

        public MainPage()
        {
            this.InitializeComponent();
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

        private void BtnExit_Click(object sender, RoutedEventArgs e)
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

        private void MainMenueBtn_Click(object sender, RoutedEventArgs e)
        {
            var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
            {
                GameEngine.CurrentGameState = 0;
            });
            PauseMenu.Visibility = Visibility.Collapsed;
            StartMenu.Visibility = Visibility.Visible;
        }

        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e){}
    }
}
