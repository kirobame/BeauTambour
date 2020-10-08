using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    /// <summary>
    /// A timed effect.
    /// </summary>
    public abstract class Feedback : SerializedMonoBehaviour
    {
        /// <summary>
        /// Notification when the effect has ended.
        /// </summary>
        public event Action onCompletion;

        /// <summary>
        /// Allows any initialization to be made.
        /// </summary>
        public virtual void Prepare() { }
        
        /// <summary>
        /// Get the timed effect which can be played inside a <code>Coroutine</code> by a <code>FeedbackPlayer</code>.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator GetRoutine();
        
        /// <summary>
        /// Allows the owning <code>FeedbackPlayer</code> to determine if a an alteration in the <code>Feedback</code> sequence must be made.
        /// </summary>
        /// <param name="currentIndex">The index at which the <code>FeedbackPlayer</code> is at in the <code>Feedback</code> sequence.</param>
        /// <param name="registry">The <code>Feedback</code> sequence.</param>
        /// <param name="nextIndex">The index indicating the <code>FeedbackPlayer</code> where it should continue in the sequence.</param>
        /// <returns></returns>
        public virtual bool GetNextIndex(int currentIndex, IReadOnlyList<Feedback> registry, out int nextIndex)
        {
            nextIndex = currentIndex + 1;
            return nextIndex < registry.Count;
        }

        protected void Complete() => onCompletion.Invoke();
    }
}