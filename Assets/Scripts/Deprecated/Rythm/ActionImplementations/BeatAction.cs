using System;
using System.Collections;
using System.Collections.Generic;
using Flux;

namespace BeauTambour
{
    public class BeatAction : RythmAction
    {
        public BeatAction(int duration, int startOffset, Action<int> action)
        {
            this.duration = duration;
            this.startOffset = startOffset;

            this.action = action;
        }

        private int duration;
        private int startOffset;

        private Action<int> action;
        
        public override void Tick(double time) { }
        public override void Beat(int count)
        {
            if (count >= startOffset) action(count - startOffset);
            if (count >= duration)
            {
                IsDone = true;
                return;
            }
        }
    }
}