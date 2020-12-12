using System;

namespace BeauTambour
{
    public class AmpleAction : RythmAction
    {
        public AmpleAction(int duration, double startOffset, Action<float> tickAction, Action<int> beatAction)
        {
            this.duration = duration;
            this.startOffset = startOffset;
            
            this.tickAction = tickAction;
            this.beatAction = beatAction;
        }
        
        private int duration;
        private double startOffset;
        
        private Action<float> tickAction;
        private Action<int> beatAction;
        
        public override void Tick(double time) { if (time >= startOffset) tickAction((float)(time - startOffset)); }
        public override void Beat(int count)
        {
            var roundedStartOffset = (int)Math.Round(startOffset);
            if (count >= roundedStartOffset) beatAction(count - roundedStartOffset);
            if (count >= duration)
            {
                IsDone = true;
                return;
            }
        }
    }
}