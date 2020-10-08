using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orion
{
    public class LoopEffect : Feedback
    {
        [SerializeField, Min(1)] private int repetitions;
        [SerializeField, Min(1)] private int range;

        private int countdown;

        public override void Prepare() => countdown = repetitions;

        public override IEnumerator GetRoutine()
        {
            Complete();
            yield break;
        }

        public override bool GetNextIndex(int currentIndex, IReadOnlyList<Feedback> registry, out int nextIndex)
        {
            countdown--;
            if (countdown > 0)
            {
                range = Mathf.Clamp(range, 0, currentIndex);
                nextIndex = currentIndex - range;

                return true;
            }
            else return base.GetNextIndex(currentIndex, registry, out nextIndex);
        }
    }
}