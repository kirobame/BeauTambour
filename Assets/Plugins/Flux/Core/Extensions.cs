using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    public static class Extensions
    {
        public static string GetNiceName(this Enum value) => $"{value.GetType().GetNiceName()}.{value}";
        public static string GetNiceName(this Type type) => type.FullName.Replace('+', '.');

        public static int IndexOf<T>(this IEnumerable<T> collection, T value)
        {
            var index = 0;
            foreach (var item in collection)
            {
                if (item.Equals(value)) return index;
                index++;
            }

            return -1;
        }
        
        public static Vector2 ToX(this float value) => new Vector2(value, 0f);
        public static Vector2 ToY(this float value) => new Vector2(0f, value);

        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Rect Enlarge(this Rect rect, float amount)
        {
            rect.position -= Vector2.one * amount;
            rect.size += Vector2.one * (amount * 2f);

            return rect;
        }
        public static Rect Pad(this Rect rect, Vector4 padding)
        {
            rect = PadHorizontally(rect, new Vector2(padding.x, padding.y));
            rect = PadVertically(rect, new Vector2(padding.z, padding.w));
            return rect;
        }
        public static Rect PadHorizontally(this Rect rect, Vector2 padding)
        {
            rect.x += padding.x;
            rect.width -= padding.y * 2f;
            return rect;
        }
        public static Rect PadVertically(this Rect rect, Vector2 padding)
        {
            rect.y += padding.x;
            rect.height -= padding.y * 2f;
            return rect;
        }
        
        public static bool TryProjectOnto(this Vector2 pt, Vector2 p1, Vector2 p2, out Vector2 result)
        {
            var U = (pt.x - p1.x) * (p2.x - p1.x) + (pt.y - p1.y) * (p2.y - p1.y);
            var UDenom = Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2);

            U /= UDenom;

            result.x = p1.x + U * (p2.x - p1.x);
            result.y = p1.y + U * (p2.y - p1.y);

            float minX, maxX, minY, maxY;

            minX = Mathf.Min(p1.x, p2.x);
            maxX = Mathf.Max(p1.x, p2.x);
        
            minY =  Mathf.Min(p1.y, p2.y);
            maxY =  Mathf.Max(p1.y, p2.y);

            if (result.x >= minX && result.x <= maxX && result.y >= minY && result.y <= maxY) return true;
            else return false;
        }
    }
}