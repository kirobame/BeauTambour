using System.Collections;
using UnityEngine;

namespace Orion
{
    public class PauseEffect : Feedback
    {
        [SerializeField] private IProxy<float> durationProxy;
        private float duration
        {
            get => durationProxy.Read();
            set => durationProxy.Write(value);
        }

        public override IEnumerator GetRoutine()
        {
            yield return new WaitForSeconds(duration);
            Complete();
        }
    }
}