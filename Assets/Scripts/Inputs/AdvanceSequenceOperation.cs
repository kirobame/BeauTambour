using UnityEngine;

namespace BeauTambour
{
    public class AdvanceSequenceOperation : RythmOperation
    {
        private InputSequence sequence;
        private int index;
        
        public void SetData(InputSequence sequence, int index)
        {
            this.sequence = sequence;
            this.index = index;
        }

        protected override bool TryGetAction(out IRythmQueueable action)
        {
            var copiedIndex = index;
            action = new BeatAction(0, 0, beat =>
            {
                if (sequence.Advancement + 1 == index) Debug.Log($"Advancing {sequence} to : {copiedIndex}");
                sequence.Advance(index);
            });

            return true;
        }
    }
}