using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion
{
    /// <summary>
    /// Represents the presence of a specific <code>AudioFilter</code> <code>Component</code> on an <code>AudioSource</code> playing an <code>AudioBundle</code>.
    /// </summary>
    [HideReferenceObjectPicker, HideLabel]
    public abstract class AudioFilter 
    {
        /// <summary>
        /// Adds the <code>AudioFilter</code> <code>Component</code> with the correct values.
        /// </summary>
        /// <param name="target">The <code>GameObject</code> possessing the <code>AudioSource</code> to be modified.</param>
        public abstract void AddTo(GameObject target);

        /// <summary>
        /// Removes the <code>AudioFilter</code> <code>Component</code>.
        /// </summary>
        /// <param name="target">The <code>GameObject</code> possessing the <code>AudioSource</code> to be modified.</param>
        public abstract void RemoveFrom(GameObject target);

        /// <summary>
        /// Assigns the values of the <code>AudioFilter</code> to an already existing related <code>Component</code>.
        /// </summary>
        /// <param name="target">The <code>GameObject</code> possessing the <code>AudioSource</code> to be modified.</param>
        public abstract void AssignValues(GameObject target);
    }
}