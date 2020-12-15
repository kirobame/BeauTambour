using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewAdvanceSequenceOperation", menuName = "Beau Tambour/Operations/Advance Sequence")]
    public class AdvanceGameplaySequenceOperation : AdvanceSequenceOperation<GameplaySequenceKeys, GameplaySequenceElement, GameplaySequence>
    {
        private static bool isRight;
        
        public override void OnStart(EventArgs inArgs)
        {
            var frogAnimator = Repository.GetSingle<Animator>(Reference.Frog);
            frogAnimator.SetTrigger(isRight ? "PlayingDrum1" : "PlayingDrum2");
            isRight = !isRight;
            
            base.OnStart(inArgs);
        }
    }
}