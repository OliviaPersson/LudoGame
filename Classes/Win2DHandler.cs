using LudoGame.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;

namespace LudoGame
{
    public class Win2DHandler
    {
        CanvasBitmap backgroundImage, blackholeImage, menuBackGroundImage, greenBaseImg, blueBaseImg, redBaseImg, yellowBaseImg, whiteTileImg, yellowTileImg, blueTileImg, greenTileImg, redTileImg;

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
            float width = (float)MainPage.bounds.Width;
            float height = (float)MainPage.bounds.Height;

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

        /// <summary>
        /// Not yet implemented Calculate positions and draw tiles
        /// </summary>
        public void DrawMap(CanvasAnimatedDrawEventArgs args)
        {

        }

        // Load Asset
        public async Task CreateResources(CanvasAnimatedControl sender)
        {
            // Menu
            menuBackGroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/menubg.png"));

            // Playing
            backgroundImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/bg.png"));
            blackholeImage = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/blackhole.png"));

            //Load map
            greenBaseImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/GreenBase.png"));
            blueBaseImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/BlueBase.png"));
            redBaseImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/RedBase.png"));
            yellowBaseImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/YellowBase.png"));
            whiteTileImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/WhiteTile.png"));
            yellowTileImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/YellowTile.png"));
            blueTileImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/BlueTile.png"));
            greenTileImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/GreenTile.png"));
            redTileImg = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/Images/Tiles/RedTile.png"));
        }
    }
}
