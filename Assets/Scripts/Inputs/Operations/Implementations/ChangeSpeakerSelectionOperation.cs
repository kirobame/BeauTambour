using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewChangeSpeakerSelectionOperation", menuName = "Beau Tambour/Operations/Change Speaker Selection")]
    public class ChangeSpeakerSelectionOperation : PhaseBoundOperation
    {
        protected SpeakerSelectionPhase AssociatedPhase => phaseHandler.Get<SpeakerSelectionPhase>(phase);
        
        [SerializeField] private int modification;
        
        protected override void RelayedOnStart(EventArgs inArgs)
        {
            base.RelayedOnStart(inArgs);
            
            var modifiedIndex = AssociatedPhase.SpeakerIndex + modification;
            AssociatedPhase.SelectSpeaker(modifiedIndex);
        }
    }
}