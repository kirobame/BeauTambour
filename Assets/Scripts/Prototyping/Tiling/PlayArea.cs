using System;
using System.Collections;
using System.Collections.Generic;
using BeauTambour.Prototyping;
using Ludiq.OdinSerializer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion.Prototyping
{
    public class PlayArea : MonoBehaviour
    {
        public Vector2 Origin => (Vector2)transform.position + tileSize * 0.5f;
        public Vector2Int Size => new Vector2Int(tiles.GetLength(0), tiles.GetLength(1));

        public Vector2 TileSize => tileSize;
        
        [SerializeField] private Vector2 tileSize;
        [SerializeField] private Vector2Int size;

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
                    tiles[x,y] = new Tile(Translate(index), index);
                }
            }
        }

        public Vector2Int Translate(Vector2 position) => MathBt.Floor((position - Origin).Divide(tileSize));
        public Vector2 Translate(Vector2Int index) => Origin + index.Scale(tileSize);
    }
}