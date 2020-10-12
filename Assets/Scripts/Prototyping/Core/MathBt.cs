using UnityEngine;

namespace BeauTambour.Prototyping
{
    public static class MathBt
    {
        public static double Clamp01(double value) => Clamp(value, 0d, 1d);
        public static double Clamp(double value, double min, double max)
        {
            if (value < min) value = min;
            else if (value > max) value = max;

            return value;
        }

        public static Vector2Int Clamp(this Vector2Int value, Vector2Int min, Vector2Int max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            return value;
        }

        public static Vector2Int Floor(Vector2 position)
        {
            var flooredVector = new Vector2Int();
            flooredVector.x = Mathf.FloorToInt(position.x);
            flooredVector.y = Mathf.FloorToInt(position.y);

            return flooredVector;
        }

        public static Vector2 Scale(this Vector2Int value, Vector2 operand)
        {
            var result = new Vector2();
            result.x = value.x * operand.x;
            result.y = value.y * operand.y;

            return result;
        }
        public static Vector2 Divide(this Vector2 value, Vector2 operand)
        {
            value.x /= operand.x;
            value.y /= operand.y;

            return value;
        }

        public static bool IsInRange(this Vector2Int value, Vector2Int min, Vector2Int max)
        {
            var xIsInRange = value.x >= min.x && value.x <= max.x;
            var yIsInRange = value.y >= min.y && value.y <= max.y;
            return xIsInRange && yIsInRange;
        }
    }
}