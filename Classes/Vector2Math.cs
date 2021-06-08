using System;
using System.Numerics;

namespace LudoGame.Classes
{
    public static class Vector2Math
    {
        public static float Magnitude(Vector2 vector)
        {
            return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2 Normalized(Vector2 vector)
        {
            float magnitude = Magnitude(vector);
            return new Vector2(vector.X / magnitude, vector.Y / magnitude);
        }
    }
}