using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace BeauTambour
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        [Space, SerializeField] private AudioMixer mixer;
        [SerializeField] private string prefix;

        void Awake()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
            
            mixer.GetFloat($"{prefix}Volume", out var volume);
            slider.value = Mathf.InverseLerp(-80.0f, 20.0f, volume);
        }

        void OnValueChanged(float value)
        {
            mixer.SetFloat($"{prefix}Volume", Mathf.Lerp(-80.0f, 20.0f, value));
        }
    }
}