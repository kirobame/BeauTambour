using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewMouseHandler", menuName = "Beau Tambour/Inputs/Handlers/Mouse")]
    public class MouseHandler : InputHandler<Vector2>
    {
        private float radius;
        private Vector2 center;
        
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);

            var drawingPool = Repository.GetSingle<DrawingPool>(Pool.Drawing);
            var camera = Repository.GetSingle<Camera>(Reference.Camera);

            center = camera.WorldToScreenPoint(drawingPool.transform.position);

            var offset = drawingPool.transform.position + Vector3.right * drawingPool.transform.localScale.x;
            radius = Vector2.Distance(center, camera.WorldToScreenPoint(offset));
        }

        public override bool OnPerformed(Vector2 input)
        {
            if (!base.OnPerformed(input)) return false;
            
            var result = (input - center) / radius;
            if (result.magnitude > 1) result.Normalize();
            
            Prolong(new Vector2EventArgs(result));
            return true;
        }
    }
}