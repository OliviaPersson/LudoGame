﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace LudoGame
{
    public class Scaler
    {
        public static void SetScale()
        {
            //Gather display information
            MainPage.scaleWidth = (float)MainPage.bounds.Width / MainPage.GameWidth;
            MainPage.scaleHeight = (float)MainPage.bounds.Height / MainPage.GameHeight;
        }

        /// <summary>
        /// Scale image file based on display information
        /// </summary>
        public static Transform2DEffect Img(CanvasBitmap source)
        {
            return ScaleImage(source, MainPage.scaleWidth, MainPage.scaleHeight);
        }

        /// <summary>
        /// Stretch image to fill in the whole window
        /// </summary>
        public static Transform2DEffect Stretch(CanvasBitmap source)
        {
            return ScaleImage(source, (float)(MainPage.bounds.Width / source.Size.Width), (float)(MainPage.bounds.Height / source.Size.Height));
        }

        /// <summary>
        /// Scale image to fit in the current window
        /// </summary>
        public static Transform2DEffect Fit(CanvasBitmap source)
        {
            float scale = MathF.Min((float)(MainPage.bounds.Width / source.Size.Width), (float)(MainPage.bounds.Height / source.Size.Height));
            return ScaleImage(source, scale, scale);
        }

        /// <summary>
        /// Scale image to fill the whole window
        /// </summary>
        public static Transform2DEffect Fill(CanvasBitmap source)
        {
            float scale = MathF.Max((float)(MainPage.bounds.Width / source.Size.Width), (float)(MainPage.bounds.Height / source.Size.Height));
            return ScaleImage(source, scale, scale);
        }
        
        /// <summary>
        /// Scales the image according to scaleX and Y
        /// </summary>
        /// <param name="source"></param>
        /// <param name="scaleY"></param>
        /// <param name="scaleX"></param>
        /// <returns></returns>
        private static Transform2DEffect ScaleImage(CanvasBitmap source, float scaleX, float scaleY)
        {
            Transform2DEffect image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(scaleX, scaleY);
            return image;
        }
    }
}