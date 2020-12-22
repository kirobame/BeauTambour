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
        
        private Cue cue => dialogue[advancement];

        private Dialogue dialogue;
        private int advancement;
        private Actor actor;

        private void Awake()
        {
            Repository.Reference(this, References.DialogueHandler);
            
            Event.Open<Cue>(GameEvents.OnNextCue);
            Event.Open<Dialogue>(GameEvents.OnDialogueFinished);
        }

        public void Play(Dialogue dialogue)
        {
            this.dialogue = dialogue;
            
            advancement = -1;
            actor = Actor.None;
            
            Continue();
        }

        public void Continue()
        {
            advancement++;
            if (advancement >= dialogue.Length) 
            {
                End();
                return;
            }

            var newActor = cue.Actor;

            var characters = Repository.GetAll<Character>(References.Characters);
            var character = characters.First(item => item.Actor == newActor);
            character.SetupDialogueHolder(holder);
            
            holder.Reboot();
            holder.TextMesh.ForceMeshUpdate();
            var info = holder.TextMesh.GetTextInfo(Regex.Replace(cue.Text, "\\<(.*?)\\>", string.Empty));

            holder.TextMesh.enabled = false;
            holder.TextMesh.enabled = true;
            
            var height = info.lineInfo.First().lineHeight * info.lineCount;
            var maximumWidth = info.lineInfo.Max(line => line.maxAdvance);
            var size = new Vector2(maximumWidth, height);
            
            holder.SetText(cue.Text);
            
            if (newActor != actor) holder.Place(character.GetPositionForDialogueHolder(), size);
            else holder.Resize(size);

            actor = newActor;
            Event.Call<Cue>(GameEvents.OnNextCue, cue);
        }

        public void End() => Event.Call<Dialogue>(GameEvents.OnDialogueFinished, dialogue);
    }
}