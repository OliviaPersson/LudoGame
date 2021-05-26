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
        public void Draw(CanvasAnimatedDrawEventArgs args, CanvasAnimatedControl sender)
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
    }
}
