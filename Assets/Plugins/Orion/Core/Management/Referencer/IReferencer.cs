using UnityEngine;

namespace Orion
{
    /// <summary>
    /// Represents a global link to a value which can be accessed by Asset referencing.
    /// </summary>
    public interface IReferencer
    {
        /// <summary>
        /// The reference.
        /// </summary>
        object Content { get; }
    }
}