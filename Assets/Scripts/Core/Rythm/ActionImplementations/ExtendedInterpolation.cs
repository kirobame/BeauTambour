using System;

namespace BeauTambour
{
    public class ExtendedInterpolation : Interpolation
    {
        public ExtendedInterpolation(int duration, double startOffset, Action onStart, Action<float> action, Action onEnd) : base(duration, startOffset, action)
        {
            this.onStart = onStart;
            this.onEnd = onEnd;
        }

        private Action onStart;
        private Action onEnd;

        protected override void OnStart()
        {
            base.OnStart();
            onStart.Invoke();
        }
        protected override void OnEnd()
        {
            base.OnEnd();
            onEnd();
        }
    }
}