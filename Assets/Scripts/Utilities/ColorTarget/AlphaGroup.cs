using System.Linq;
using UnityEngine;

namespace BeauTambour
{
    public class AlphaGroup : AlphaTarget
    {
        [SerializeField] private AlphaTarget[] targets;

        public override void Prepare(float goal)
        {
            base.Prepare(goal);
            foreach (var target in targets) target.Prepare(goal);
        }
        public override void Set(float ratio)
        {
            foreach (var target in targets) target.Set(ratio);
        }
    }
}