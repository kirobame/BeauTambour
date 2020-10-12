using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    [Flags]
    public enum Shape
    {
        None = 0,
        Square = 1,
        Triangle = 2,
        Circle = 4,
        Wave = 8,
        Rectangle = 16,
        Cloud = 32,
        Star = 64,
        Moon = 128,
        Nine = 256,
        Ten = 512
    }
}