using System;
using UnityEngine;

namespace Orion
{
    /// <summary>
    /// Allows to trigger event which can be listened to globally by Asset referencing.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRemoteVoidEvent", menuName = "Orion/Remote/Event/Void", order = -6000)]
    public class RemoteVoidEvent : ScriptableObject
    {
        public event Action Call
        {
            add => call += value;
            remove => call -= value;
        }
        private event Action call;

        /// <summary>
        /// Call the event.
        /// </summary>
        public void Invoke() => call?.Invoke();
    }
}