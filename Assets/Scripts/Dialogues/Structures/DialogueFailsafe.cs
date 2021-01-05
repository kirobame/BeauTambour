using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeauTambour
{
    public class DialogueFailsafe
    {
        public DialogueFailsafe(string name)
        {
            this.name = name;
            
            availableIndices = new List<int>();
            onHoldIndex = -1;
            
            dialogues = new List<Dialogue[]>();
        }

        public bool TryAddOption(string name, string[] texts)
        {
            var entry = new Dialogue[texts.Length];
            for (var i = 0; i < texts.Length; i++) entry[i] = Dialogue.Parse(name, texts[i]);
            
            availableIndices.Add(availableIndices.Count);
            dialogues.Add(entry);

            return true;
        }

        public string Name => name;
        private string name;
        
        private List<int> availableIndices;
        private int onHoldIndex;
        
        private List<Dialogue[]> dialogues;

        public Dialogue GetDialogue()
        {
            var index = availableIndices[Random.Range(0, availableIndices.Count)];
            var dialogue = dialogues[index][(int)GameState.UsedLanguage];

            availableIndices.Remove(index);
            if (onHoldIndex != -1)
            {
                availableIndices.Add(onHoldIndex);
                onHoldIndex = -1;
            }

            if (!availableIndices.Any())
            {
                if (dialogues.Count > 1)
                {
                    onHoldIndex = index;
                    for (var i = 0; i < dialogues.Count; i++)
                    {
                        if (i == index) continue;
                        availableIndices.Add(i);
                    }
                }
                else for (var i = 0; i < dialogues.Count; i++) availableIndices.Add(i);
            }
            
            return dialogue;
        }
    }
}