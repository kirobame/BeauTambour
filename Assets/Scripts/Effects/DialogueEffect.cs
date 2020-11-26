using System.Collections.Generic;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [ItemPath("Beau Tambour/Dialogue")]
    public class DialogueEffect : Effect
    {
        [SerializeField] private bool trim;
        [SerializeField] private int from;
        [SerializeField] private int to;
        
        public DialogueReference reference;
        private Dialogue dialogue;

        void Awake() => Event.Register(DialogueProvider.EventType.OnDialoguesDownloaded, CacheDialogue);
        
        public override void Initialize()
        {
            base.Initialize();

            var manager = Repository.GetSingle<DialogueManager>(Reference.DialogueManager);
            manager.BeginDialogue(dialogue);
            
            Event.Register(DialogueManager.EventType.OnEnd, End);
        }

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            if (!IsDone)
            {
                prolong = false;
                return advancement;
            }
            else
            {
                Event.Unregister(DialogueManager.EventType.OnEnd, End);
                return base.Evaluate(advancement, registry, deltaTime, out prolong);
            }
        }
        
        private void CacheDialogue()
        {
            if (trim)
            {
                var value = reference.Value;
                dialogue = value.Trim(new Vector2Int(from, to));
            }
            else dialogue = reference.Value;
        }

        private void End() => IsDone = true;
    }
}