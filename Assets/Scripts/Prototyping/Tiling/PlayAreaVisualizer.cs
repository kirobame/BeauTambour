using System;
using Orion;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace BeauTambour.Prototyping
{
    public class PlayAreaVisualizer : MonoBehaviour
    {
        [SerializeField] private PlayArea playArea;
        [SerializeField] private Rectangle tileVisualPrefab;
        [SerializeField] private Transform parent;

        [Space, SerializeField] private float lineDashSpace;
        [SerializeField] private float lineThickness;
        
        [Space, SerializeField, Range(0.1f, 1f)] private float sizeRatio;

        void Awake() => playArea.OnGeneration += BuildVisuals;

        void OnEnable() => RenderPipelineManager.endCameraRendering += PostRender;
        void OnDisable() => RenderPipelineManager.endCameraRendering -= PostRender;
        
        private void BuildVisuals()
        {
            //var startBlockX = Repository.Get<BlockGenerator>().StartX - 1;
            for (var x = 0; x < playArea.Size.x; x++)
            {
                //if (x == 1 || x == startBlockX) continue;
                
                for (var y = 0; y < playArea.Size.y; y++)
                {
                    var position = playArea[x, y].Position;
                    
                    var tileVisual = Instantiate(tileVisualPrefab, position, Quaternion.identity, parent);
                    tileVisual.Width = playArea.TileSize.x * sizeRatio;
                    tileVisual.Height = playArea.TileSize.y * sizeRatio;
                }
            }
        }

        void PostRender(ScriptableRenderContext context, Camera camera)
        {
            if (!playArea.IsActive) return;
            
            var x = Repository.Get<BlockGenerator>().StartX - 1;
            var position = playArea.Translate(new Vector2Int(x, 0));

            Draw.LineThicknessSpace = ThicknessSpace.Meters;
            Draw.LineGeometry = LineGeometry.Flat2D;
            Draw.LineThickness = lineThickness;
            var dashStyle = DashStyle.DefaultDashStyleLine;
            dashStyle.spacing = lineDashSpace;

            Draw.LineDashed(position - Vector2.up * 500, position + Vector2.up * 100, dashStyle, LineEndCap.None, UnityEngine.Color.white);
            
            position =  playArea.Translate(new Vector2Int(2, 0));
            Draw.LineDashed(position - Vector2.up * 500, position + Vector2.up * 100, dashStyle, LineEndCap.None, UnityEngine.Color.white);
        }
    }
}