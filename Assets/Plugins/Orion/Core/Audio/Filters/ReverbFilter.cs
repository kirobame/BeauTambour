using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class ReverbFilter : AudioFilter
    {
        [SerializeField] private AudioReverbPreset preset;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioReverbFilter>();
            AssignValues(component);
        }
        
        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioReverbFilter>();
            Object.Destroy(component);
        }
        
        public override void AssignValues(GameObject target) => AssignValues(target.GetComponent<AudioReverbFilter>());

        private void AssignValues(AudioReverbFilter component)
        {
            component.reverbPreset = preset;
        }
    }
}