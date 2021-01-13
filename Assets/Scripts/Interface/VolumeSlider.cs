using System;
using Flux;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace BeauTambour
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Vector2 range = new Vector2(-20.0f, 15.0f);
        
        [Space, SerializeField] private AudioMixer mixer;
        [SerializeField] private string prefix;

        private string fullName => $"{prefix}Volume";
        
        void Awake()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
            
            mixer.GetFloat(fullName, out var volume);
            volume = Repository.GetPersistent<float>(volume, fullName);
            mixer.SetFloat(fullName, volume);
            
            slider.value = Mathf.InverseLerp(range.x, range.y, volume);
        }
        void OnDestroy()
        {
            mixer.GetFloat(fullName, out var volume);
            Repository.SetPersistent(volume, fullName);
        }

        void OnValueChanged(float value)
        {
            mixer.SetFloat(fullName, Mathf.Lerp(range.x, range.y, value));
        }
    }
}