using System;

namespace Deprecated
{
    public class TickAction : RythmAction
    {
        public TickAction(int duration, double startOffset, Action<float> action)
        {
            this.duration = duration;
            this.startOffset = startOffset;
            
            this.action = action;
        }
        
        private int duration;
        private double startOffset;
        
        private Action<float> action;
        
        public override void Tick(double time)
        {
            if (time >= startOffset) action((float)(time - startOffset));
            if (time >= duration)
            {
                IsDone = true;
                return;
            }
        }
        public override void Beat(int count) { }
    }
}