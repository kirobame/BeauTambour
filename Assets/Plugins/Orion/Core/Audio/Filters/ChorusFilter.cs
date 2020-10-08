using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class ChorusFilter : AudioFilter
    {
        [SerializeField, Range(0f, 1f)] private float dryMix;
        [SerializeField, Range(0f, 1f)] private float wetMix01;
        [SerializeField, Range(0f, 1f)] private float wetMix02;
        [SerializeField, Range(0f, 1f)] private float wetMix03;
        [SerializeField, Min(0.1f)] private float delay = 0.1f;
        [SerializeField, Range(0f, 20f)] private float rate;
        [SerializeField, Range(0f, 1f)] private float depth;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioChorusFilter>();
            AssignValues(component);
        }
        
        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioChorusFilter>();
            Object.Destroy(component);
        }

        public override void AssignValues(GameObject target) => AssignValues(target.GetComponent<AudioChorusFilter>());

        private void AssignValues(AudioChorusFilter component)
        {
            component.dryMix = dryMix;
            component.wetMix1 = wetMix01;
            component.wetMix2 = wetMix02;
            component.wetMix3 = wetMix03;
            component.delay = delay;
            component.rate = rate;
            component.depth = depth;
        }
    }
}