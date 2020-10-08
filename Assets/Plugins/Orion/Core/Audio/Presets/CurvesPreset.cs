using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    public class CurvesPreset : AudioPreset
    {
        [SerializeField, HideReferenceObjectPicker] private AnimationCurve volumeCurve = AnimationCurve.Linear(0f,1f,1f,0f);
        [SerializeField, HideReferenceObjectPicker] private AnimationCurve spatialBlendCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);
        [SerializeField, HideReferenceObjectPicker] private AnimationCurve spreadCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);
        [SerializeField, HideReferenceObjectPicker] private AnimationCurve reverbZoneMixCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        
        public override void Apply(AudioSource source)
        {
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, volumeCurve);
            source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, spatialBlendCurve);
            source.SetCustomCurve(AudioSourceCurveType.Spread, spreadCurve);
            source.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, reverbZoneMixCurve);
        }
    }
}