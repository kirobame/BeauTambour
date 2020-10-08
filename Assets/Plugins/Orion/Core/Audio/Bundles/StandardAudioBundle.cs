using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    [CreateAssetMenu(fileName = "NewStandardAudioBundle", menuName = "Orion/Audio/Bundles/Standard")]
    public class StandardAudioBundle : AudioBundle
    {
        [SerializeField, PropertyOrder(-1)] private AudioClip clip;

        protected override AudioClip GetClip() => clip;
    }
}