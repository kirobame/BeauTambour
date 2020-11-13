using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewRuntimeSettings", menuName = "Beau Tambour/Settings")]
    public class RuntimeSettings : ScriptableObject
    {
        public SupportedLanguage Language => language;
        [SerializeField] private SupportedLanguage language;
    }
}