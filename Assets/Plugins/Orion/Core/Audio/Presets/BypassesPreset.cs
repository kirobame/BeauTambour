using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class BypassesPreset : AudioPreset
    {
        [SerializeField] private bool bypassEffects;
        [SerializeField] private bool bypassListenerEffects;
        [SerializeField] private bool bypassReverbZones;
        
        public override void Apply(AudioSource source)
        {
            source.bypassEffects = bypassEffects;
            source.bypassListenerEffects = bypassListenerEffects;
            source.bypassReverbZones = bypassReverbZones;
        }
    }
}