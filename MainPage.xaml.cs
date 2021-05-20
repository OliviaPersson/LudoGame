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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static CanvasBitmap backgroundImage, blackholeImage;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
        public static float GameWidth = 1920;
        public static float GameHeight = 1080;
        public static float scaleWidth, scaleHeight;

        // Create instance of GameEngine
        GameEngine gsm = new GameEngine();

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            Scaler.SetScale();
            // Init all GameStates
            gsm.GameStateInit();
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Scaler.SetScale();
        }

        private void GameCanvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            gsm.Update();
        }
        private void GameCanvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            gsm.Draw(args);
        }

        private void GameCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if(gsm.CurrentGameState == 0)
            {
                var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
                {
                    gsm.CurrentGameState = 1;
                });
            }
            if (gsm.CurrentGameState == 1)
            {
                var action = GameCanvas.RunOnGameLoopThreadAsync(() =>
                {
                    gsm.CurrentGameState = 0;
                });
            }
        }

        private void GameCanvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(gsm.CreateResources(sender).AsAsyncAction());
        }

        private void GameCanvas_Tapped(object sender, TappedRoutedEventArgs e){}
    }
}
