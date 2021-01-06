using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField] private DialogueHolder holder;
        [SerializeField] private float widthCorrection;

        [Space, SerializeField] private DialogueEvent[] events;
        private Dictionary<string, DialogueEvent> eventRegistry;
        private DialogueEvent playedEvent;

        public ISpeaker Speaker { get; private set; }
        private Actor actor;
        
        private Cue cue => dialogue[advancement];

        private Dialogue dialogue;
        private int advancement;

        private Queue<Dialogue> queue;
        private bool isPlaying;

        private void Awake()
        {
            Repository.Reference(this, References.DialogueHandler);
            
            queue = new Queue<Dialogue>();
            
            Event.Open<Cue>(GameEvents.OnNextCue);
            Event.Open<Dialogue>(GameEvents.OnDialogueFinished);
            
            eventRegistry = new Dictionary<string, DialogueEvent>();
            foreach (var dialogueEvent in events) eventRegistry.Add(dialogueEvent.Key, dialogueEvent);
        }

        public void Enqueue(Dialogue dialogue)
        {
            if (isPlaying) queue.Enqueue(dialogue);
            else Assign(dialogue);
        }
        private void Assign(Dialogue dialogue)
        {
            this.dialogue = dialogue;
            
            advancement = -1;
            actor = Actor.None;

            isPlaying = true;
            Continue();
        }

        public void Continue()
        {
            if (!isPlaying) return;
            
            advancement++;
            if (advancement >= dialogue.Length) 
            {
                if (queue.Count > 0)
                {
                    var dialogue = queue.Dequeue();
                    Assign(dialogue);
                }
                else End();
                
                return;
            }

            var newActor = cue.Actor;

            var character = Extensions.GetCharacter<Character>(cue.Actor);
            if (character == null)
            {
                Continue();
                return;
            }
            character.SetupDialogueHolder(holder);
            
            Speaker = character as ISpeaker;
            Speaker.BeginTalking();

            holder.Bootup();
            holder.TextMesh.ForceMeshUpdate();
            var info = holder.TextMesh.GetTextInfo(Regex.Replace(cue.Text, "\\<(.*?)\\>", string.Empty));

            holder.TextMesh.enabled = false;
            holder.TextMesh.enabled = true;

            var lineSpacing = holder.TextMesh.lineSpacing / 100.0f;
            var height = 0f;
            
            for (var i = 0; i < info.lineCount; i++)
            {
                var ascend = info.lineInfo[i].ascender - info.lineInfo[i].baseline;
                var descend = info.lineInfo[i].baseline - info.lineInfo[i].descender;
                var strictLineHeight = ascend + descend;

                height += strictLineHeight;

                if (i + 1 < info.lineCount)
                {
                    height += info.lineInfo[i].lineHeight - strictLineHeight;
                    height += lineSpacing;
                }
            }
            height += holder.TextMesh.margin.y + holder.TextMesh.margin.w;

            var maximumWidth = info.lineInfo.Max(line => line.maxAdvance);
            maximumWidth += holder.TextMesh.margin.x + holder.TextMesh.margin.z;
            maximumWidth -= widthCorrection;
            
            var size = new Vector2(maximumWidth, height);

            if (newActor != actor) holder.Renew(character.GetPositionForDialogueHolder(), size, cue.Text);
            else holder.Refresh(size, cue.Text);

            actor = newActor;
            Event.Call<Cue>(GameEvents.OnNextCue, cue);
        }

        public void End()
        {
            holder.Deactivate();
            isPlaying = false;

            if (eventRegistry.TryGetValue(dialogue.Name, out playedEvent))
            {
                playedEvent.OnEnd += OnEventEnd;
                playedEvent.Execute(this);
            }
            else
            {
                Event.Call<Dialogue>(GameEvents.OnDialogueFinished, dialogue);

                var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
                phaseHandler.Play(PhaseCategory.SpeakerSelection);
            }
        }

        void OnEventEnd()
        {
            playedEvent.OnEnd -= OnEventEnd;
            
            Event.Call<Dialogue>(GameEvents.OnDialogueFinished, dialogue);

            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            phaseHandler.Play(PhaseCategory.SpeakerSelection);
        }
    }
}