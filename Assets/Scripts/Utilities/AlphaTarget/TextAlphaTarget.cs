using TMPro;
using UnityEngine;

namespace BeauTambour
{
    public class TextAlphaTarget : AlphaTarget
    {
        [SerializeField] private TMP_Text text;

        private float start;

        public override void Prepare(float goal)
        {
            base.Prepare(goal);
            start = text.alpha;
        }
        public override void Set(float ratio)
        {
            var col = text.color;
            col.a = Mathf.Lerp(start, goal, ratio);
            text.color = col;
        }
    }
}