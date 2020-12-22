using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewRuntimeSettings", menuName = "Beau Tambour/Editor/Settings")]
    public class RuntimeSettings : ScriptableObject
    {
        public SupportedLanguage Language => language;
        [SerializeField] private SupportedLanguage language;

        [SerializeField] private int clarityFailThreshold;
        
        public int GlobalClarity => globalClarity;
        private int globalClarity;
        private int failCount;

        void ResetGlobalClarity() => globalClarity = 0;
        void RegisterFail()
        {
            failCount++;
            if (failCount >= clarityFailThreshold)
            {
                failCount = 0;
                globalClarity++;
            }
        }
    }
}