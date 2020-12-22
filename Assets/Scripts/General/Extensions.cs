using System;

namespace BeauTambour
{
    public static class Extensions
    {
        public static int GetEnumCount<T>() where T : Enum => Enum.GetNames(typeof(T)).Length;
    }
}