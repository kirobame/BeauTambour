using UnityEngine;

namespace Deprecated
{
    public class CanvasGroupTarget : ColorTarget
    {
        public override Color StartingColor => Color.white;
        public override float StartingAlpha => canvasGroup.alpha;

        [SerializeField] private CanvasGroup canvasGroup;
        
        public override void Set(Color color) { }
        public override void SetAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }
    }
}