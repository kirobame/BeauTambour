using System;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace Orion.Prototyping
{
    public class PlayAreaVisualizer : MonoBehaviour
    {
        [SerializeField] private PlayArea playArea;
        
        [Space, SerializeField] private float tileCornerRadius;
        [SerializeField] private Color tileColor;

        void OnEnable () => RenderPipelineManager.endCameraRendering += PostRender;
        void OnDisable () => RenderPipelineManager.endCameraRendering -= PostRender;
        
        void PostRender(ScriptableRenderContext context, Camera camera)
        {

            for (var x = 0; x < playArea.Size.x; x++)
            {
                for (var y = 0; y < playArea.Size.y; y++)
                {
                    Draw.Rectangle(playArea[x,y].Position, Vector3.forward, playArea.TileSize * 0.95f, tileCornerRadius, tileColor);
                }
            }
        }
    }
}