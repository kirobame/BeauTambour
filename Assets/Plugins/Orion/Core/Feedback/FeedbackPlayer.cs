using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    /// <summary>
    /// Utility <code>Component</code> allowing the simple execution of many <code>Feedback</code> effects.
    /// </summary>
    public class FeedbackPlayer : SerializedMonoBehaviour
    {
        #if UNITY_EDITOR

        private void RemoveFeedback(Feedback feedback)
        {
            feedbacks.Remove(feedback);
            DestroyImmediate(feedback);
        }
        
        #endif
        
        [ListDrawerSettings(HideAddButton = true, CustomRemoveElementFunction = "RemoveFeedback")]
        [SerializeField] private List<Feedback> feedbacks = new List<Feedback>();

        private Coroutine routine;
        private int currentIndex = -1;
        
        /// <summary>
        /// If no sequence is ongoing : 
        /// Begin playing the <code>Feedback</code> sequence from the start.
        /// </summary>
        [Button]
        public void Play()
        {
            if (!feedbacks.Any()) return;
            
            Stop();
            currentIndex = 0;

            foreach (var item in feedbacks) item.Prepare();
            
            var feedback = feedbacks.First();
            feedback.onCompletion += PlayNext;
            
            routine = StartCoroutine(feedback.GetRoutine());
        }
        /// <summary>
        /// If a sequence is ongoing :
        /// Stop its execution.
        /// </summary>
        public void Stop()
        {
            if (routine != null) StopCoroutine(routine);
            
            currentIndex = -1;
            routine = null;
        }

        private void PlayNext()
        {
            feedbacks[currentIndex].onCompletion -= PlayNext;
            if (!feedbacks[currentIndex].GetNextIndex(currentIndex, feedbacks, out currentIndex))
            {
                Stop();
                return;
            }

            feedbacks[currentIndex].onCompletion += PlayNext;
            routine = StartCoroutine(feedbacks[currentIndex].GetRoutine());
        }
    }
}