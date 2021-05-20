using LudoGame.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace LudoGame
{
    public class Win2DHandler
    {
        CanvasBitmap backgroundImage, blackholeImage, menuBackGroundImage;

        public void Update()
        {
            switch (GameEngine.CurrentGameState)
            {
                case 0: break;
                case 1: break;
                case 2: break;
                case 3: break;
                default: break;
            }
        }
        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            switch (GameEngine.CurrentGameState)
            {
                case 0:
                    args.DrawingSession.DrawImage(Scaler.Img(menuBackGroundImage));
                    break;
                case 1:
                    args.DrawingSession.DrawImage(Scaler.Img(backgroundImage));
                    args.DrawingSession.DrawImage(Scaler.Img(blackholeImage), 50, 50);
                    break;
                case 2: break;
                case 3: break;
            }
        }
        // Load Asset
        public async Task CreateResources(CanvasAnimatedControl sender)
        {
            // Menu
            menuBackGroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/menubg.png"));

            // Playing
            backgroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/bg.png"));
            blackholeImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/blackhole.png"));
        }
    }
}
