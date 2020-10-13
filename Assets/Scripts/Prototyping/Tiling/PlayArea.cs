using System;
using System.Collections;
using System.Collections.Generic;
using Ludiq.OdinSerializer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace BeauTambour.Prototyping
{
    public class PlayArea : MonoBehaviour
    {
        public event Action OnGeneration;
        
        public Rect Bounds => new Rect(Origin, Size * tileSize);
        public RectInt IndexedBounds => new RectInt(Vector2Int.zero, Size);
        
        public Vector2 Origin => (Vector2)transform.position - new Vector2(size.x, size.y) * tileSize * 0.5f;

        public Vector2Int IntendedSize => size;
        public Vector2Int Size => new Vector2Int(tiles.GetLength(0), tiles.GetLength(1));

        public Vector2 TileSize => tileSize;
        
        [SerializeField] private Vector2 tileSize;
        [SerializeField] private Vector2Int size;

        private HashSet<ITilable> tilables = new HashSet<ITilable>();
        private Tile[,] tiles = new Tile[0,0];

        public Tile this[Vector2Int index] => tiles[index.x, index.y];
        public Tile this[int x, int y] => tiles[x, y];

        [Button]
        public void Generate()
        {
            tiles = new Tile[size.x, size.y];
            for (var x = 0; x < size.x; x++)
            {
                for (var y = 0; y < size.y; y++)
                {
                    var index = new Vector2Int(x, y);
                    tiles[x,y] = new Tile(Translate(index) + tileSize * 0.5f, index);
                }
            }

            OnGeneration?.Invoke();
        }

        public bool Register(ITilable tilable)
        {
            if (!tilables.Add(tilable)) return false;

            var tile = this[Translate(tilable.Position)];
            tile.Add(tilable);
            
            tilable.Tile = tile;
            tilable.OnMove += OnTilableMoved;
            
            return true;
        }
        public bool Unregister(ITilable tilable)
        {
            if (!tilables.Remove(tilable)) return false;

            tilable.OnMove -= OnTilableMoved;
            return true;
        }

        public Vector2Int Translate(Vector2 position) => MathBt.Floor((position - Origin).Divide(tileSize));
        public Vector2 Translate(Vector2Int index) => Origin + index.Scale(tileSize);

        private void OnTilableMoved(ITilable tilable)
        {
            var previousTile = tilable.Tile;
            var index = Translate(tilable.Position);

            if (index.IsInRange(Vector2Int.zero, Size - Vector2Int.one))
            {
                var tile = this[Translate(tilable.Position)];

                if (previousTile == tile) return;

                previousTile?.Remove(tilable);
                tile.Add(tilable);
                tilable.Tile = tile;
            }
            else
            {
                if (previousTile == null) return;
                
                previousTile.Remove(tilable);
                tilable.Tile = null;
            }
        }
    }
}