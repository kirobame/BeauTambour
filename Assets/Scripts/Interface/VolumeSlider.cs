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

        void Awake()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
            
            mixer.GetFloat($"{prefix}Volume", out var volume);
            slider.value = Mathf.InverseLerp(range.x, range.y, volume);
        }

        void OnValueChanged(float value)
        {
            mixer.SetFloat($"{prefix}Volume", Mathf.Lerp(range.x, range.y, value));
        }
    }
}