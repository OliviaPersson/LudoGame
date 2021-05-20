﻿using LudoGame.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using System.Drawing;
using System.Numerics;

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

            switch (GameEngine.CurrentGameState)
            {
                case 0:
                    args.DrawingSession.DrawImage(Scaler.Img(menuBackGroundImage));
                    break;
                case 1:
                    args.DrawingSession.DrawImage(Scaler.Img(backgroundImage));
                    DrawMap(args);
                    break;
                case 2: break;
                case 3: break;
            }
        }

        /// <summary>
        /// Calculate positions and draw tiles
        /// </summary>
        public void DrawMap(CanvasAnimatedDrawEventArgs args)
        {
            float width = (float)MainPage.bounds.Width;
            float height = (float)MainPage.bounds.Height;

            args.DrawingSession.DrawImage(Scaler.Img(blackholeImage), (float)(width / 2.15), (float)(height / 2.35));
            args.DrawingSession.DrawImage(Scaler.Img(yellowBaseImg), width / 4, height / 10);
            args.DrawingSession.DrawImage(Scaler.Img(blueBaseImg), width / 4, (float)(height / 1.3));
            args.DrawingSession.DrawImage(Scaler.Img(greenBaseImg), (float)(width / 1.4), (float)(height / 10));
            args.DrawingSession.DrawImage(Scaler.Img(redBaseImg), (float)(width / 1.4), (float)(height / 1.3));

            double angle = 369.0 / 40 * Math.PI / 180.0;
            double distance = Math.Max(width / 5, height / 5);
            Point center = new Point(Convert.ToInt32(width / 2), Convert.ToInt32(height / 2));

            for (int i = 0; i < 40; i++)
            {
                int x = center.X + Convert.ToInt32(Math.Cos(angle * i) * distance);
                int y = center.Y + Convert.ToInt32(Math.Sin(angle * i) * distance);

                switch (i)
                {
                    case 5:
                        args.DrawingSession.DrawImage(Scaler.Img(redTileImg), x, y);

                        Point redPosition = new Point(x, y);
                        Vector2 direction = new Vector2(redPosition.X - center.X, redPosition.Y - center.Y);
                        /*
                        for (int j = 0; j < 4; j++)
                        {
                            double redAngle = Math.Abs(Math.Atan2((x - center.X), (y - center.Y)) * (180.0 / Math.PI));
                            int redX = direction.X + Convert.ToInt32(Math.Cos(redAngle * j) * distance);
                            int redY = (direction.Y + Convert.ToInt32(Math.Sin(redAngle * j) * distance;

                            args.DrawingSession.DrawImage(Scaler.Img(redTileImg), redX, redY);
                        }
                        */
                            break;
                    case 15:
                        args.DrawingSession.DrawImage(Scaler.Img(blueTileImg), x, y);
                        break;
                    case 25:
                        args.DrawingSession.DrawImage(Scaler.Img(yellowTileImg), x, y);
                        break;
                    case 35:
                        args.DrawingSession.DrawImage(Scaler.Img(greenTileImg), x, y);
                        break;
                    default:
                        args.DrawingSession.DrawImage(Scaler.Img(whiteTileImg), x, y);
                        break;
                }
                    
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
