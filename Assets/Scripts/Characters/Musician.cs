using System;
using System.Collections.Generic;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewMusician", menuName = "Beau Tambour/Characters/Musician")]
    public class Musician : Character, ISpeaker
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
                
                if (data.ContainsKey("NeededAttributes")) neededAttributes = data["NeededAttributes"].Split('/');
                else neededAttributes = new string[0];
                
                if (data.ContainsKey("GivenAttributes")) givenAttributes = data["GivenAttributes"].Split('/');
                else givenAttributes = new string[0];
                
                dialogues = new Dialogue[texts.Length];
                for (var i = 0; i < texts.Length; i++) dialogues[i] = Dialogue.Parse(name, texts[i]);

                return true;
            }

            public string Name => name;
            private string name;
            
            public IReadOnlyList<string> NeededAttributes => neededAttributes;
            private string[] neededAttributes;

            public Emotion RequiredEmotion => requiredEmotion;
            private Emotion requiredEmotion;
            
            public string Failsafe => failsafe;
            private string failsafe;

            public IReadOnlyList<string> Childs => childs;
            private string[] childs;

            public IReadOnlyList<string> GivenAttributes => givenAttributes;
            private string[] givenAttributes;
            
            public Dialogue GetDialogue() => dialogues[(int)GameState.UsedLanguage];
            private Dialogue[] dialogues;
        }

        #endregion
        
        public override RuntimeCharacter RuntimeLink => CastedRuntimeLink;
        public RuntimeMusician CastedRuntimeLink { get; private set; }

        public AudioCharMapPackage AudioCharMap => audioCharMap;
        [Space, SerializeField] private AudioCharMapPackage audioCharMap;

        private List<string> rootNodeKeys;
        private Dictionary<string, DialogueNode> nodes;
        private Dictionary<string, DialogueFailsafe> failsafes;
        private HashSet<string> attributes;

        private DialogueNode currentNode;

        #region Dialogue Initialization
        
        public void AddDialogueNodeRootKey(string name, int blockIndex)
        {
            var difference = blockIndex - (rootNodeKeys.Count - 1);
            if (difference > 0) for (var i = 0; i < difference; i++) rootNodeKeys.Add("Empty");

            rootNodeKeys[blockIndex] = name;
        }
        public void AddDialogueNode(DialogueNode node) => nodes.Add(node.Name, node);
        public void AddDialogueFailsafe(string name, string[] texts)
        {
            if (failsafes.ContainsKey(name)) failsafes[name].TryAddOption(this.name, texts);
            else
            {
                var dialogueFailsafe = new DialogueFailsafe(name);
                if (dialogueFailsafe.TryAddOption(this.name, texts)) failsafes.Add(name, dialogueFailsafe);
            }
        }
        #endregion

        public override void Bootup(RuntimeCharacter runtimeCharacter)
        {
            base.Bootup(runtimeCharacter);
            CastedRuntimeLink = (RuntimeMusician) runtimeCharacter;

            GameState.RegisterSpeakerForUse(this);
            
            rootNodeKeys = new List<string>();
            nodes = new Dictionary<string, DialogueNode>();
            failsafes = new Dictionary<string, DialogueFailsafe>();
            attributes = new HashSet<string>();

            Event.Register(GameEvents.OnBlockPassed, OnBlockPassed);
        }

        public Dialogue[] GetDialogues(Emotion emotion)
        {
            if (currentNode.Childs[0] == "Empty") return new Dialogue[] { failsafes[currentNode.Failsafe].GetDialogue() };
            
            Debug.Log($"[{Actor}] : {currentNode.Name}");
            foreach (var childName in currentNode.Childs)
            {
                var child = nodes[childName];
                if (child.RequiredEmotion == emotion && attributes.IsSupersetOf(child.NeededAttributes))
                {
                    foreach (var attribute in child.GivenAttributes) attributes.Add(attribute);
                    currentNode = child;
                    
                    if (child.Childs[0] == "Empty")
                    {
                        Debug.Log($"End of narrative arc for : {name}");
                        if (GameState.NotifyMusicianArcEnd(out var blockDialogue))
                        {
                            return new Dialogue[]
                            {
                                child.GetDialogue(),
                                blockDialogue
                            };
                        }
                    }
                    
                    return new Dialogue[] { child.GetDialogue() };
                }
            }
            
            return new Dialogue[] { failsafes[currentNode.Failsafe].GetDialogue() };
        }

        void ISpeaker.BeginTalking() => CastedRuntimeLink.Intermediary.BeginTalking();
        void ISpeaker.StopTalking() => CastedRuntimeLink.Intermediary.StopTalking();

        void ISpeaker.PlayMelodyFor(Emotion emotion) => CastedRuntimeLink.Intermediary.PlayMelodyFor(emotion);
        
        void OnBlockPassed()
        {
            if (rootNodeKeys.Count == 0) return;
            
            Debug.Log($"{name} : {rootNodeKeys[GameState.BlockIndex]}");
            currentNode = nodes[rootNodeKeys[GameState.BlockIndex]];
        }
    }
}