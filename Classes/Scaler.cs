using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace LudoGame
{
    public class Scaler
    {
        private static float _uniformScale;
        private static float _fillScale;
        public static void SetScale()
        {
            //Gather display information
            MainPage.scaleWidth = (float)MainPage.bounds.Width / MainPage.GameWidth;
            MainPage.scaleHeight = (float)MainPage.bounds.Height / MainPage.GameHeight;
            _uniformScale = MathF.Min(MainPage.scaleWidth, MainPage.scaleHeight);
            _fillScale = MathF.Max(MainPage.scaleWidth, MainPage.scaleHeight);
        }

        /// <summary>
        /// Scale image file based on display information
        /// </summary>
        public static Transform2DEffect Img(CanvasBitmap source)
        {
            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(MainPage.scaleWidth, MainPage.scaleHeight);
            return image;
        }

        /// <summary>
        /// Scale image file uniformly based on display information
        /// </summary>
        public static Transform2DEffect UniformImg(CanvasBitmap source)
        {
            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(_uniformScale, _uniformScale);
            return image;
        }

        /// <summary>
        /// Scale image file uniformly based on display information
        /// </summary>
        public static Transform2DEffect FillImg(CanvasBitmap source)
        {
            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(_fillScale, _fillScale);
            return image;
        }
    }
}