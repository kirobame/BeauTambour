using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        public static Vector2 ProjectOnto(this Vector2 pt, Vector2 p1, Vector2 p2)
        {
            var closest = ProjectOnto(pt, p1, p2, out var code);
            return closest;
        }
        public static Vector2 ProjectOnto(this Vector2 pt, Vector2 p1, Vector2 p2, out int code)
        {
            var dx = p2.x - p1.x;
            var dy = p2.y - p1.y;

            var t = ((pt.x - p1.x) * dx + (pt.y - p1.y) * dy) / (dx * dx + dy * dy);

            if (t < 0)
            {
                code = 1;
                return p1;
            }
            else if (t > 1)
            {
                code = 2;
                return p2;
            }
            else
            {
                code = 3;
                return new Vector2(p1.x + t * dx, p1.y + t * dy);
            }
        }

        public static T[] Split<T>(this T value) where T : Enum
        {
            var results = Enum.GetValues(typeof(T)).Cast<T>().Where(item => value.HasFlag(item)).ToArray();
            return results;
        }
        public static int Count<T>(this T value) where T : Enum
        {
            var count = Enum.GetValues(typeof(T)).Cast<T>().Count(item => value.HasFlag(item));
            return count;
        }
    }
}