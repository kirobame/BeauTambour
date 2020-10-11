using System;
using UnityEngine;

namespace Orion.Prototyping
{
    public interface ITilable
    {
        event Action<ITilable> OnMove;

        Tile Tile { get; set; }

        TilableType Type { get; }
        Vector2 Position { get; }
    }
}