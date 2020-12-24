using System;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewSkipCueOperation", menuName = "Beau Tambour/Operations/Skip Cue")]
    public class SkipCueOperation : PhaseBoundOperation
    {
        private bool canSkip;
        
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);

            Event.Register<Cue>(GameEvents.OnNextCue, cue => canSkip = false);
            Event.Register(GameEvents.OnCueFinished, () => canSkip = true);

            Event.Open(GameEvents.OnCueSkipped);
        }

        protected override void RelayedOnStart(EventArgs inArgs)
        {
            if (!canSkip) return;

            Event.Call(GameEvents.OnCueSkipped);
            
            var dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            dialogueHandler.Continue();
        }
    }
}