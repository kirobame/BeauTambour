using System;
using UnityEngine;

namespace Deprecated
{
    public class Interpolation : RythmAction
    {
        public Interpolation(int duration, double startOffset, Action<float> action)
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
            if (time >= startOffset) action(Mathf.Clamp01((float)(time - startOffset) / (float)(duration - startOffset)));
        }
        public override void Beat(int count)
        {
            if (count == startOffset) OnStart();
            if (count >= duration)
            {
                IsDone = true;
                OnEnd();
                
                return;
            }
        }

        protected virtual void OnStart() => action(0f);
        protected virtual void OnEnd() => action(1f);
    }
}