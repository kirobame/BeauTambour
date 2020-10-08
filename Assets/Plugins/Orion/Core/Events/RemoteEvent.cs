using System;
using UnityEngine;

namespace Orion
{
    /// <summary>
    /// Allows to trigger event which can be listened to globally by Asset referencing.
    /// </summary>
    /// <typeparam name="T">The first parameter type of the event.</typeparam>
    public abstract class RemoteEvent<T> : ScriptableObject
    {
        public event Action<T> Call
        {
            add => call += value;
            remove => call -= value;
        }
        private event Action<T> call;

        /// <summary>
        /// Call the event.
        /// </summary>
        public void Invoke(T value) => call?.Invoke(value);
    }
    
    /// <summary>
    /// Allows to trigger event which can be listened to globally by Asset referencing.
    /// </summary>
    /// <typeparam name="T1">The first parameter type of the event.</typeparam>
    /// <typeparam name="T2">The second parameter type of the event.</typeparam>
    public abstract class RemoteEvent<T1,T2> : ScriptableObject
    {
        public event Action<T1,T2> Call
        {
            add => call += value;
            remove => call -= value;
        }
        private event Action<T1,T2> call;

        /// <summary>
        /// Call the event.
        /// </summary>
        public void Invoke(T1 arg1, T2 arg2) => call?.Invoke(arg1, arg2);
    }
    
    /// <summary>
    /// Allows to trigger event which can be listened to globally by Asset referencing.
    /// </summary>
    /// <typeparam name="T1">The first parameter type of the event.</typeparam>
    /// <typeparam name="T2">The second parameter type of the event.</typeparam>
    /// <typeparam name="T3">The third parameter type of the event.</typeparam>
    public abstract class RemoteEvent<T1,T2,T3> : ScriptableObject
    {
        public event Action<T1,T2,T3> Call
        {
            add => call += value;
            remove => call -= value;
        }
        private event Action<T1,T2,T3> call;

        /// <summary>
        /// Call the event.
        /// </summary>
        public void Invoke(T1 arg1, T2 arg2, T3 arg3) => call?.Invoke(arg1, arg2, arg3);
    }
}