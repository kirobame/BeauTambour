using System.Collections.Generic;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [ItemPath("Beau Tambour/Dialogue")]
    public class DialogueEffect : Effect
    {
        public DialogueReference reference;
        private Dialogue dialogue;

        private int cueIndex;

        void Awake() => Event.Register(DialogueProvider.EventType.OnDialoguesDownloaded, CacheDialogue);
        
        public override void Initialize()
        {
            base.Initialize();

            Event.Register(OperationEvent.Skip, Next);
            cueIndex = -1;
        }

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            if (cueIndex == -1) Next();

            if (cueIndex < dialogue.Cues.Count)
            {
                prolong = false;
                return advancement;
            }
            else
            {
                Event.Unregister(OperationEvent.Skip, Next);
                return base.Evaluate(advancement, registry, deltaTime, out prolong);
            }
        }
        
        private void CacheDialogue() => dialogue = reference.Value;
        private void Next()
        {
            cueIndex++;
            if (cueIndex >= dialogue.Cues.Count) return;
            
            Debug.Log($"For {reference.Id} : [{cueIndex + 1}/{dialogue.Cues.Count}]{dialogue.Cues[cueIndex]}");
        }
    }
}