using LudoGame.Classes;
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
                    args.DrawingSession.DrawImage(Scaler.Img(GameEngine.menuBackGroundImage));
                    break;
                case 1:
                    args.DrawingSession.DrawImage(Scaler.Img(GameEngine.backgroundImage));
                    DrawMap(args);
                    break;
                case 2: args.DrawingSession.DrawImage(Scaler.Img(GameEngine.backgroundImage)); break;
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

            args.DrawingSession.DrawImage(Scaler.Img(GameEngine.blackholeImage), (float)(width / 2.15), (float)(height / 2.35));
            args.DrawingSession.DrawImage(Scaler.Img(GameEngine.yellowBaseImg), width / 4, height / 10);
            args.DrawingSession.DrawImage(Scaler.Img(GameEngine.blueBaseImg), width / 4, (float)(height / 1.3));
            args.DrawingSession.DrawImage(Scaler.Img(GameEngine.greenBaseImg), (float)(width / 1.4), (float)(height / 10));
            args.DrawingSession.DrawImage(Scaler.Img(GameEngine.redBaseImg), (float)(width / 1.4), (float)(height / 1.3));

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
                        args.DrawingSession.DrawImage(Scaler.Img(GameEngine.redTileImg), x, y); 

                        /// <summary>
                        /// Not yet working Draw final tiles to center
                        /// </summary>
                        /*
                        Point redPosition = new Point(x, y);
                        Vector2 direction = new Vector2(redPosition.X - center.X, redPosition.Y - center.Y);
                       
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
                        args.DrawingSession.DrawImage(Scaler.Img(GameEngine.blueTileImg), x, y);
                        break;
                    case 25:
                        args.DrawingSession.DrawImage(Scaler.Img(GameEngine.yellowTileImg), x, y);
                        break;
                    case 35:
                        args.DrawingSession.DrawImage(Scaler.Img(GameEngine.greenTileImg), x, y);
                        break;
                    default:
                        args.DrawingSession.DrawImage(Scaler.Img(GameEngine.whiteTileImg), x, y);
                        break;
                }
                    
            }
        }
    }
}
