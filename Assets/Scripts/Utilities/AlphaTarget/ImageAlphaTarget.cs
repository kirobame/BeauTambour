using UnityEngine;
using UnityEngine.UI;

namespace BeauTambour
{
    public class ImageAlphaTarget : AlphaTarget
    {
        private float start;
        
        [SerializeField] private Image image;

        public override void Prepare(float goal)
        {
            base.Prepare(goal);
            start = image.color.a;
        }
        public override void Set(float ratio)
        {
            var color = image.color;
            color.a = Mathf.Lerp(start, goal, ratio);
            image.color = color;
        }
    }
}