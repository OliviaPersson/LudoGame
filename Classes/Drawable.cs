using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.Foundation;

namespace LudoGame.Classes
{
    public class Drawable
    {
        public CanvasBitmap Bitmap { get; set; }
        public bool isHover = false;
        public bool isHidden = false;

        /// <summary>
        /// Position is based on a virtual coordinatesystem where center window is (0,0),
        /// and the smallest height or width edge of the window is at 1000 and -1000 on that axis.
        /// The larger axis will have 1000 and -1000 at the same distance from the center as
        /// the smaller axis.
        /// Position is centered on the image, so an image at (0,0) will always be centered in the window.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                if (_setActualPosition) ActualPosition = value;
                else CalculateActualPosition();
            }
        }

        public float ImageSize { get; set; }
        public Scale Scaling { get; set; }
        public Vector2 ActualPosition { get; private set; }

        public Vector2 ActualCenter
        {
            get
            {
                return new Vector2(ActualPosition.X + ScaledSize.X / 2, ActualPosition.Y + ScaledSize.Y / 2);
            }
        }

        public Vector2 ScaledSize
        {
            get
            {
                Rect image = Scaling(Bitmap, ImageSize).GetBounds(Bitmap);
                return new Vector2((float)image.Width, (float)image.Height);
            }
        }

        private Vector2 _position;
        private bool _setActualPosition = false;

        public delegate Transform2DEffect Scale(CanvasBitmap source, float sizeMultiplier);

        /// <summary>
        /// Use this to create a drawable that follows the virtual coordinatesystem
        /// </summary>
        /// <param name="bitmap">The image that is displayed when this is drawn</param>
        /// <param name="position">The position based on (0,0) being center of window</param>
        /// <param name="imageSize">The image size multiplier</param>
        /// <param name="Scaling">The scaling methood to use when drawing this object</param>
        public Drawable(CanvasBitmap bitmap, Vector2 position, float imageSize, Scale Scaling)
        {
            this.Bitmap = bitmap;
            this.ImageSize = imageSize;
            this.Scaling = Scaling;
            this.Position = position;
        }

        /// <summary>
        /// use this to with positionIsActualPosition = true to set Position = ActualPosition
        /// </summary>
        /// <param name="bitmap">The image that is displayed when this is drawn</param>
        /// <param name="position">The position of the image</param>
        /// <param name="imageSize">The image size multiplier</param>
        /// <param name="Scaling">The scaling methood to use when drawing this object</param>
        /// <param name="positionIsActualPosition">set to true to use the conventional position as (0,0) being top left</param>
        public Drawable(CanvasBitmap bitmap, Vector2 position, float imageSize, Scale Scaling, bool positionIsActualPosition)
        {
            this.Bitmap = bitmap;
            this.ImageSize = imageSize;
            this.Scaling = Scaling;
            _setActualPosition = positionIsActualPosition;
            this.Position = position;
        }

        public void CalculateActualPosition()
        {
            if (!_setActualPosition)
            {
                float minBounds = MathF.Min((float)MainPage.bounds.Width, (float)MainPage.bounds.Height);
                ActualPosition = new Vector2((float)MainPage.bounds.Width / 2 + (Position.X * (minBounds / MainPage.gameWidth / 2) - (ScaledSize.X / 2)), (float)MainPage.bounds.Height / 2 - (Position.Y * (minBounds / MainPage.gameHeight / 2) + (ScaledSize.Y / 2)));
            }
        }
    }
}