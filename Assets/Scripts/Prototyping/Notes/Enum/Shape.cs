using System;
using System.Collections;
using System.Collections.Generic;

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
        Cross = 16,
        Cloud = 32,
        Star = 64,
        Moon = 128,
        Heart = 256,
        Flower = 512
    }
}