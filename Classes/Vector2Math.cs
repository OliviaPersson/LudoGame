using System;
using System.Numerics;

namespace LudoGame.Classes
{
    public static class Vector2Math
    {

        /// <summary>
        /// Returns the length of a Vector2
        /// </summary>
        public static float Magnitude(Vector2 vector)
        {
            return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        /// <summary>
        /// Returns the Vector2 in the same direction but with a length of 1
        /// </summary>
        public static Vector2 Normalized(Vector2 vector)
        {
            float magnitude = Magnitude(vector);
            return new Vector2(vector.X / magnitude, vector.Y / magnitude);
        }
    }
}