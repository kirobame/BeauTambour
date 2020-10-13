using System;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace BeauTambour.Prototyping
{
    public class PlayAreaVisualizer : MonoBehaviour
    {
        [SerializeField] private PlayArea playArea;
        [SerializeField] private Rectangle tileVisualPrefab;
        [SerializeField] private Transform parent;
        
        [Space, SerializeField, Range(0.1f, 1f)] private float sizeRatio;

        void Awake() => playArea.OnGeneration += BuildVisuals;

        private void BuildVisuals()
        {
            for (var x = 0; x < playArea.Size.x; x++)
            {
                for (var y = 0; y < playArea.Size.y; y++)
                {
                    var position = playArea[x, y].Position;
                    
                    var tileVisual = Instantiate(tileVisualPrefab, position, Quaternion.identity, parent);
                    tileVisual.Width = playArea.TileSize.x * sizeRatio;
                    tileVisual.Height = playArea.TileSize.y * sizeRatio;
                }
            }
        }
    }
}