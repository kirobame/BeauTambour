using System;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public interface ITilable
    {
        event Action<ITilable> OnMove;

        Tile Tile { get; set; }
        Vector2 Position { get; set; }

        TilableType Type { get; }
        object Link { get; }
    }
}