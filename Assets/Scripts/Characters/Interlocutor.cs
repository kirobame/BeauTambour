using System.Collections.Generic;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewInterlocutor", menuName = "Beau Tambour/Characters/Interlocutor")]
    public class Interlocutor : Character
    {
        #region Encapsulated Types

        public class DialogueBlock
        {
            public string Name => name;
            private string name = "Empty";
            
            public Emotion Emotion => emotion;
            private Emotion emotion;
            
            public Dialogue[] Dialogues => dialogues;
            private Dialogue[] dialogues;

            public void Set(string name, Emotion emotion, string[] texts)
            {
                this.name = name;
                this.emotion = emotion;
               
                dialogues = new Dialogue[texts.Length];
                for (var i = 0; i < texts.Length; i++) dialogues[i] = Dialogue.Parse(name, texts[i]);
            }
        }
        #endregion

        public override bool HasArcEnded => false;
        public override int Branches => 1;

        public float SingingPitch => singingPitch;
        [SerializeField] private float singingPitch;
        
        private List<DialogueBlock> blocks;
        private List<Dictionary<Emotion, DialogueFailsafe>> options;

        private bool hasEntered;
        
        #region Dialogue Initialization

        public void AddBlockDialogue(string name, Emotion emotion, string[] texts, int blockIndex)
        {
            var difference = blockIndex - (blocks.Count - 1);
            if (difference > 0) for (var i = 0; i < difference; i++) blocks.Add(new DialogueBlock());

            blocks[blockIndex].Set(name, emotion, texts);
        }
        public void AddDialogueOption(string name, Emotion emotion, string[] texts, int blockIndex)
        {
            var difference = blockIndex - (options.Count - 1);
            if (difference > 0) for (var i = 0; i < difference; i++) options.Add(new Dictionary<Emotion, DialogueFailsafe>());
            
            if (options[blockIndex].ContainsKey(emotion)) options[blockIndex][emotion].TryAddOption(name, texts);
            else
            {
                var dialogueOption = new DialogueFailsafe(emotion.ToString());
                if (dialogueOption.TryAddOption(name, texts)) options[blockIndex].Add(emotion, dialogueOption);
            }
        }
        #endregion
        
        public override void Bootup(RuntimeCharacterBase runtimeCharacter)
        {
            base.Bootup(runtimeCharacter);

            blocks = new List<DialogueBlock>();
            options = new List<Dictionary<Emotion, DialogueFailsafe>>();
        }

        public override bool IsValid(Emotion emotion, out int selection, out int followingBranches)
        {
            selection = 0;
            followingBranches = 0;
            
            var block = blocks[GameState.BlockIndex];
            if (block.Emotion == emotion) return true;

            followingBranches = 0;
            return false;
        }
        public override Dialogue[] GetDialogues(Emotion emotion)
        {
            var block = blocks[GameState.BlockIndex];
            if (block.Emotion == emotion)
            {
                GameState.PassBlock();
                return new Dialogue[] { block.Dialogues[(int)GameState.UsedLanguage] };
            }
            else return new Dialogue[] { options[GameState.BlockIndex][emotion].GetDialogue() };
        }
        
        protected override void OnBlockPassed()
        {
            if (GameState.BlockIndex >= blocks.Count) return;
            if (blocks[GameState.BlockIndex].Name != "Empty") base.OnBlockPassed();
        }
    }
}