﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class NoteAttribute { }
    
    [CreateAssetMenu(fileName = "NewMusician", menuName = "Beau Tambour/Characters/Musician")]
    public class Musician : Character
    {
        #region Encapsulated Types
        
        public class DialogueNode
        {
            public DialogueNode(string name) => this.name = name;
            
            public bool TryProcess(string[] texts, Dictionary<string, string> data)
            {
                if (!data.ContainsKey("Emotion")) return false;
                else requiredEmotion = (Emotion)Enum.Parse(typeof(Emotion), data["Emotion"]);
                
                if (!data.ContainsKey("Failsafe")) return false;
                else failsafe = data["Failsafe"];

                if (!data.ContainsKey("Childs")) return false;
                else childs = data["Childs"].Split('/');

                dialogues = new Dialogue[texts.Length];
                for (var i = 0; i < texts.Length; i++) dialogues[i] = Dialogue.Parse(texts[i]);

                return true;
            }

            public string Name => name;
            private string name;

            public Emotion RequiredEmotion => requiredEmotion;
            private Emotion requiredEmotion;
            
            public string Failsafe => failsafe;
            private string failsafe;

            public IReadOnlyList<string> Childs => childs;
            private string[] childs;
            
            public Dialogue GetDialogue() => dialogues[(int)GameState.UsedLanguage];
            private Dialogue[] dialogues;
        }

        #endregion
        
        public override RuntimeCharacter RuntimeLink => CastedRuntimeLink;
        public RuntimeMusician CastedRuntimeLink { get; private set; }

        private List<string> rootNodeKeys;
        private Dictionary<string, DialogueNode> nodes;
        private Dictionary<string, DialogueFailsafe> failsafes;

        private DialogueNode currentNode;

        #region Dialogue Initialization
        
        public void AddDialogueNodeRootKey(string name, int phaseIndex)
        {
            var difference = phaseIndex - (rootNodeKeys.Count - 1);
            if (difference > 0) for (var i = 0; i < difference; i++) rootNodeKeys.Add("Empty");

            rootNodeKeys[phaseIndex] = name;
        }
        public void AddDialogueNode(DialogueNode node) => nodes.Add(node.Name, node);
        public void AddDialogueFailsafe(string name, string[] texts)
        {
            if (failsafes.ContainsKey(name)) failsafes[name].TryAddOption(texts);
            else
            {
                var dialogueFailsafe = new DialogueFailsafe(name);
                if (dialogueFailsafe.TryAddOption(texts)) failsafes.Add(name, dialogueFailsafe);
            }
        }
        #endregion

        public override void Inject(RuntimeCharacter runtimeCharacter)
        {
            base.Inject(runtimeCharacter);
            CastedRuntimeLink = (RuntimeMusician) runtimeCharacter;
            
            rootNodeKeys = new List<string>();
            nodes = new Dictionary<string, DialogueNode>();
            failsafes = new Dictionary<string, DialogueFailsafe>();

            Event.Register(GameEvents.OnBlockPassed, OnBlockPassed);
        }

        public Dialogue GetDialogue(Emotion emotion)
        {
            if (currentNode.Childs[0] == "Empty") return failsafes["End"].GetDialogue();
            
            foreach (var childName in currentNode.Childs)
            {
                var child = nodes[childName];
                if (child.RequiredEmotion == emotion)
                {
                    if (child.Childs[0] == "Empty") Debug.Log($"End of narrative arc for : {name}");
                    
                    currentNode = child;
                    return child.GetDialogue();;
                }
            }
            
            return failsafes[currentNode.Failsafe].GetDialogue();
        }

        void OnBlockPassed()
        {
            if (rootNodeKeys.Count == 0) return;
            currentNode = nodes[rootNodeKeys[GameState.BlockIndex]];
        }
    }
}