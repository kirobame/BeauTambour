using UnityEngine;

namespace BeauTambour
{
    public class BootstrappingIntermediary : MonoBehaviour
    {
        void Awake()
        {
            BootstrappingRelay.SetUseBackup(true);
            BootstrappingRelay.SetPlayIntro(true);
            BootstrappingRelay.SetSkipStart(false);
        }
        
        public void RelayBlockValue(int index) => BootstrappingRelay.SetStartingBlock(index);
        
        public void RelayUseBackup(bool value) => BootstrappingRelay.SetUseBackup(!value);
        public void RelayPlayIntroValue(bool value) => BootstrappingRelay.SetPlayIntro(value);
        public void RelaySkipStartValue(bool value) => BootstrappingRelay.SetSkipStart(value);
    }
}