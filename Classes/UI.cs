using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace LudoGame.Classes
{
    static class UI
    {
        private static List<GamePiece> _finishedGamePieces = new List<GamePiece>();
        private static Rectangle[] _redElements = new Rectangle[4];
        private static Rectangle[] _greenElements = new Rectangle[4];
        private static Rectangle[] _yellowElements = new Rectangle[4];
        private static Rectangle[] _blueElements = new Rectangle[4];

        public static void Initialize(Rectangle[] rectangles)
        {
            int redIndex = 0;
            int greenIndex = 0;
            int yellowIndex = 0;
            int blueIndex = 0;
            foreach (var rectangle in rectangles)
            {
                rectangle.Opacity = 0.3; // reset opacity on inizialisation
                if (rectangle.Name.Contains("Red"))
                {
                    _redElements[redIndex] = rectangle;
                    redIndex++;
                }
                else if (rectangle.Name.Contains("Green"))
                {
                    _greenElements[greenIndex] = rectangle;
                    greenIndex++;
                }
                else if (rectangle.Name.Contains("Yellow"))
                {
                    _yellowElements[yellowIndex] = rectangle;
                    yellowIndex++;
                }
                else if (rectangle.Name.Contains("Blue"))
                {
                    _blueElements[blueIndex] = rectangle;
                    blueIndex++;
                }
            }
        }

        public static void FinishGamePiece(GamePiece piece)
        {
            _finishedGamePieces.Add(piece);

            // change opacity of the next UI element
            switch (piece.race)
            {
                case GameRace.Red:
                    foreach (var item in _redElements)
                    {
                        if (item.Opacity != 1)
                        {
                            item.Opacity = 1;
                            break;
                        }
                    }
                    break;
                case GameRace.Green:
                    foreach (var item in _greenElements)
                    {
                        if (item.Opacity != 1)
                        {
                            item.Opacity = 1;
                            break;
                        }
                    }
                    break;
                case GameRace.Yellow:
                    foreach (var item in _yellowElements)
                    {
                        if (item.Opacity != 1)
                        {
                            item.Opacity = 1;
                            break;
                        }
                    }
                    break;
                case GameRace.Blue:
                    foreach (var item in _blueElements)
                    {
                        if (item.Opacity != 1)
                        {
                            item.Opacity = 1;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
