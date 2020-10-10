using UnityEngine;

namespace Orion.Prototyping
{
    public class Tile
    {
        public Tile(Vector2 position, Vector2Int index)
        {
            Position = position;
            Index = index;
        }
        
        public readonly Vector2 Position;
        public readonly Vector2Int Index;
    }
}