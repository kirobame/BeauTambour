using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Tile
    {
        public Tile(Vector2 position, Vector2Int index)
        {
            Position = position;
            Index = index;

            var tilableTypes = Enum.GetValues(typeof(TilableType)) as TilableType[];
            foreach (var tilableType in tilableTypes) groupedTilables.Add(tilableType, new List<ITilable>());
        }
        
        public readonly Vector2 Position;
        public readonly Vector2Int Index;

        private Dictionary<TilableType, List<ITilable>> groupedTilables = new Dictionary<TilableType, List<ITilable>>();

        public IReadOnlyList<ITilable> this[TilableType type] => groupedTilables[type];
        
        public bool Add(ITilable tilable)
        {
            var list = groupedTilables[tilable.Type];
            if (list.Contains(tilable)) return false;
            
            list.Add(tilable);
            return true;
        }
        public bool Remove(ITilable tilable) => groupedTilables[tilable.Type].Remove(tilable);
    }
}