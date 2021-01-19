using UnityEngine;
using UnityEngine.UI;

namespace BeauTambour
{
    public class BootstrappingIntermediary : MonoBehaviour
    {
        [SerializeField] private Toggle[] toggles;
        
        void Awake()
        {
            if (BootstrappingRelay.UseBackup != -1) toggles[0].isOn = BootstrappingRelay.UseBackup == 0;
            else
            {
                toggles[0].isOn = false;
                BootstrappingRelay.SetUseBackup(true);
            }
            
            if (BootstrappingRelay.PlayIntro != -1) toggles[1].isOn = BootstrappingRelay.PlayIntro != 0;
            else
            {
                toggles[1].isOn = true;
                BootstrappingRelay.SetPlayIntro(true);
            }
            
            if (BootstrappingRelay.SkipStart != -1) toggles[2].isOn = BootstrappingRelay.SkipStart != 0;
            else
            {
                toggles[2].isOn = false;
                BootstrappingRelay.SetSkipStart(false);
            }
            
            if (BootstrappingRelay.SkipDialogues != -1) toggles[3].isOn = BootstrappingRelay.SkipDialogues != 0;
            else
            {
                toggles[3].isOn = false;
                BootstrappingRelay.SetSkipDialogues(false);
            }
        }
        
        public void RelayBlockValue(int index) => BootstrappingRelay.SetStartingBlock(index);
        
        public void RelayUseBackup(bool value) => BootstrappingRelay.SetUseBackup(!value);
        public void RelayPlayIntroValue(bool value) => BootstrappingRelay.SetPlayIntro(value);
        public void RelaySkipStartValue(bool value) => BootstrappingRelay.SetSkipStart(value);
        public void RelaySkipDialogueValue(bool value) => BootstrappingRelay.SetSkipDialogues(value);
    }
}