using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class DistortionFilter : AudioFilter
    {
        [SerializeField, Range(0f,1f)] private float level;
        
        public override void AddTo(GameObject target)
        {
            var component = target.AddComponent<AudioDistortionFilter>();
            AssignValues(component);
        }
        
        public override void RemoveFrom(GameObject target)
        {
            var component = target.GetComponent<AudioDistortionFilter>();
            Object.Destroy(component);
        }
        
        public override void AssignValues(GameObject target) => AssignValues(target.GetComponent<AudioDistortionFilter>());

        private void AssignValues(AudioDistortionFilter component)
        {
            component.distortionLevel = level;
        }
    }
}