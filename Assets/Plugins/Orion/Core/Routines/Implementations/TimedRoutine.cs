using System;
using System.Collections;

namespace Orion
{
    public class TimedRoutine : Routine
    {
        public float duration;
        public object waitInstruction;
    
        public Action<float> during;
        public Func<float> incrementTime;

        protected override IEnumerator GetCall()
        {
            var time = 0f;
            while (time < duration)
            {
                during(time);

                yield return waitInstruction;
                time += incrementTime();
            }
        }
    }
}