using System.Collections.Generic;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewInterlocutor", menuName = "Beau Tambour/Characters/Interlocutor")]
    public class Interlocutor : Character, ISpeaker
    {
        #region Encapsulated Types

        public class DialogueBlock
        {
            public Emotion Emotion => emotion;
            private Emotion emotion;
            
            public Dialogue[] Dialogues => dialogues;
            private Dialogue[] dialogues;

            public void Set(Emotion emotion, string[] texts)
            {
                this.emotion = emotion;
               
                dialogues = new Dialogue[texts.Length];
                for (var i = 0; i < texts.Length; i++) dialogues[i] = Dialogue.Parse(texts[i]);
            }
        }
        #endregion
        
        public override RuntimeCharacter RuntimeLink => CastedRuntimeLink;
        public RuntimeInterlocutor CastedRuntimeLink { get; private set; }
        
        public AudioCharMapPackage AudioCharMap => audioCharMap;
        [Space, SerializeField] private AudioCharMapPackage audioCharMap;

        private List<DialogueBlock> blocks;
        private List<Dictionary<Emotion, DialogueFailsafe>> options;
        
        #region Dialogue Initialization

        public void AddBlockDialogue(Emotion emotion, string[] texts, int blockIndex)
        {
            var difference = blockIndex - (blocks.Count - 1);
            if (difference > 0) for (var i = 0; i < difference; i++) blocks.Add(new DialogueBlock());

            blocks[blockIndex].Set(emotion, texts);
        }
        public void AddDialogueOption(Emotion emotion, string[] texts, int blockIndex)
        {
            var difference = blockIndex - (options.Count - 1);
            if (difference > 0) for (var i = 0; i < difference; i++) options.Add(new Dictionary<Emotion, DialogueFailsafe>());
            
            if (options[blockIndex].ContainsKey(emotion)) options[blockIndex][emotion].TryAddOption(texts);
            else
            {
                var dialogueOption = new DialogueFailsafe(emotion.ToString());
                if (dialogueOption.TryAddOption(texts)) options[blockIndex].Add(emotion, dialogueOption);
            }
        }
        #endregion
        
        public override void Bootup(RuntimeCharacter runtimeCharacter)
        {
            base.Bootup(runtimeCharacter);
            CastedRuntimeLink = (RuntimeInterlocutor)runtimeCharacter;
            
            blocks = new List<DialogueBlock>();
            options = new List<Dictionary<Emotion, DialogueFailsafe>>();
        }
        
        public Dialogue[] GetDialogues(Emotion emotion)
        {
            var block = blocks[GameState.BlockIndex];
            if (block.Emotion == emotion)
            {
                Debug.Log("Go to next block");
                GameState.PassBlock();

                return new Dialogue[] { block.Dialogues[(int)GameState.UsedLanguage] };
            }
            else return new Dialogue[] { options[GameState.BlockIndex][emotion].GetDialogue() };
        }

        void ISpeaker.BeginTalking() => CastedRuntimeLink.Intermediary.BeginTalking();
        void ISpeaker.StopTalking() => CastedRuntimeLink.Intermediary.StopTalking();

        void ISpeaker.PlayMelodyFor(Emotion emotion) => RuntimeLink.Delay(() => Event.Call(GameEvents.OnNoteValidationDone), 1);
    }
}