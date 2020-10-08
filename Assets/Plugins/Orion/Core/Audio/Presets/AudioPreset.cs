using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    /// <summary>
    /// Represents specific settings for an <code>AudioSource</code>.
    /// </summary>
    [HideReferenceObjectPicker, HideLabel]
    public abstract class AudioPreset
    {
        /// <summary>
        /// Applies the settings onto the passed <code>AudioSource</code>.
        /// </summary>
        /// <param name="source">The <code>AudioSource</code> to be modified.</param>
        public abstract void Apply(AudioSource source);
    }
}