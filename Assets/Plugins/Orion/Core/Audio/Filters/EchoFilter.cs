using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class EchoFilter : AudioFilter
    {
        [SerializeField, Min(10f)] private float delay = 10f;
        [SerializeField, Range(0f,1f)] private float decayRatio;
        [SerializeField, Range(0f,1f)] private float wetMix;
        [SerializeField, Range(0f,1f)] private float dryMix;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioEchoFilter>();
            AssignValues(component);
        }
        
        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioEchoFilter>();
            Object.Destroy(component);
        }
        
        public override void AssignValues(GameObject target) => AssignValues(target.GetComponent<AudioEchoFilter>());

        private void AssignValues(AudioEchoFilter component)
        {
            component.delay = delay;
            component.decayRatio = decayRatio;
            component.wetMix = wetMix;
            component.dryMix = dryMix;
        }
    }
}