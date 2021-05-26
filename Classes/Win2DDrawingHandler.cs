using LudoGame.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI;

namespace LudoGame
{

    public static class Win2DDrawingHandler
    {
        public static void Draw(CanvasAnimatedDrawEventArgs args, Drawable[] drawables)
        {
            foreach (Drawable currentItem in drawables)
            {
                args.DrawingSession.DrawImage(currentItem.Scaling(currentItem.Bitmap, currentItem.ImageSize), currentItem.ActualPosition);
            }
        }

        public static void DrawGameTilesDebugLines(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args, GameTile[] gameTiles)
        {
            if (gameTiles != null)
            {
                foreach (var item in gameTiles)
                {
                    if (item.nextTile != null)
                    {
                        args.DrawingSession.DrawLine(item.drawable.ActualCenter, item.nextTile.drawable.ActualCenter, new CanvasSolidColorBrush(sender, Colors.Cyan));
                    }
                    if (item.nextHomeTile != null)
                    {
                        args.DrawingSession.DrawLine(item.drawable.ActualCenter, item.nextHomeTile.drawable.ActualCenter, new CanvasSolidColorBrush(sender, Colors.Magenta));
                    }
                }
            }
        }

    }
}
