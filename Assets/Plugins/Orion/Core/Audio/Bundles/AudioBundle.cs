using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion
{
    /// <summary>
    /// A collection of <code>AudioPreset</code> & <code>AudioFilter</code> which can be applied onto any <code>AudioSource</code>.
    /// </summary>
    public abstract class AudioBundle : SerializedScriptableObject
    {
        #region UNITY_EDITOR

        private IEnumerable<Type> GetAvailablePresets() => typeof(AudioPreset).GetDependencies().Where(type => presets.All(preset => preset.GetType() != type));
        private IEnumerable<Type> GetAvailableFilters() => typeof(AudioFilter).GetDependencies().Where(type => filters.All(filter => filter.GetType() != type));

        #endregion
        
        private static AudioPreset[] standardPresets = new AudioPreset[]
        {
            new SoundPreset(), 
            new OutputPreset(), 
            new BypassesPreset(), 
            new SpatializationPreset(), 
            new CurvesPreset()
        };
        
        public IReadOnlyList<AudioPreset> Presets => presets;
        public IReadOnlyList<AudioFilter> Filters => filters;
        
        [TypeFilter("GetAvailablePresets")]
        [SerializeField] private AudioPreset[] presets = new AudioPreset[0];
        
        [TypeFilter("GetAvailableFilters")]
        [SerializeField] private AudioFilter[] filters = new AudioFilter[0];

        private int[] standardIndices;

        void OnEnable()
        {
            var indices = new List<int>();
            for (var i = 0; i < standardPresets.Length; i++)
            {
                var type = standardPresets[i].GetType();
                if (presets.Any(preset => preset.GetType() == type)) indices.Add(i);
            }

            standardIndices = indices.ToArray();
        }

        /// <summary>
        /// Affects the passed <code>AudioSource</code> with all <code>AudioFilter</code> & <code>AudioPreset</code>.
        /// </summary>
        /// <param name="audioSource">The <code>AudioSource</code> to be affected.</param>
        public void AssignTo(AudioSource audioSource)
        {
            audioSource.clip = GetClip();
            
            for (var i = 0; i < standardIndices.Length; i++) standardPresets[standardIndices[i]].Apply(audioSource);
            foreach (var preset in presets) preset.Apply(audioSource);

            if (!filters.Any())
            {
                AudioFilterHanlder hanlder;
                
                if (audioSource.TryGetComponent<AudioFilterHanlder>(out hanlder)) hanlder.Clear();
                else
                {
                    hanlder = audioSource.gameObject.AddComponent<AudioFilterHanlder>();
                    hanlder.Initialize(audioSource);
                }
                
                hanlder.Set(filters);
            }
        }
        
        protected abstract AudioClip GetClip();
    }
}