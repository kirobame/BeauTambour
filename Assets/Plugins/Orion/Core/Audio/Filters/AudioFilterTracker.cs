using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orion
{
    /// <summary>
    /// Utility <code>Component</code> which handles the management of <code>AudioFilter</code> <code>Component</code> on an <code>AudioSource</code>.
    /// </summary>
    public class AudioFilterHanlder : MonoBehaviour
    {
        private AudioSource target;
        private List<AudioFilter> filters = new List<AudioFilter>();

        /// <summary>
        /// Assigns the <code>AudioSource</code> that will be handled.
        /// </summary>
        /// <param name="audioSource">The <code>AudioSource</code> to be handled.</param>
        public void Initialize(AudioSource audioSource) => target = audioSource;
        
        /// <summary>
        /// Passes all <code>AudioFilter</code> which needs to be present on the handled.<code>AudioSource</code>
        /// </summary>
        /// <param name="filters">The <code>AudioFilter</code> to be assigned.</param>
        public void Set(IReadOnlyList<AudioFilter> filters)
        {
            var indices = new List<int>();
            for (var i = 0; i < filters.Count; i++) indices.Add(i);

            var removals = new List<AudioFilter>();
            foreach (var ownedFilter in this.filters)
            {
                var matchingIndex = -1;
                for (var i = 0; i < indices.Count; i++)
                {
                    if (filters[indices[i]].GetType() != ownedFilter.GetType()) continue;
                    
                    matchingIndex = indices[i];
                    break;
                }

                if (matchingIndex == -1)
                {
                    removals.Add(ownedFilter);
                    continue;
                }
                
                filters[matchingIndex].AssignValues(target.gameObject);
                indices.Remove(matchingIndex);
            }

            foreach (var removal in removals)
            {
                removal.RemoveFrom(target.gameObject);
                this.filters.Remove(removal);
            }
            
            for (var i = 0; i < indices.Count; i++)
            {
                var filter = filters[indices[i]];
                
                filter.AddTo(target.gameObject);
                this.filters.Add(filter);
            }
        }
        
        /// <summary>
        /// Removes all present <code>AudioFilter</code> on the handled <code>AudioSource</code>.
        /// </summary>
        public void Clear()
        {
            foreach (var filter in filters) filter.RemoveFrom(target.gameObject);
            filters.Clear();
        }
    }
}