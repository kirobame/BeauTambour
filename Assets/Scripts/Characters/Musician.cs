using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewMusician", menuName = "Beau Tambour/Characters/Musician")]
    public class Musician : Character
    {
        #region Encapsulated Types
        
        public class DialogueNode
        {
            public DialogueNode(string name) => this.name = name;
            
            public bool TryProcess(string[] texts, Dictionary<string, string> data)
            {
                if (!data.ContainsKey("Emotion"))
                {
                    Debug.LogError($"No Emotion on {name}");
                    foreach (var key in data.Keys) Debug.LogError($"---| Available key : {key}");
                    
                    return false;
                }
                else
                {
                    var emotionName = data["Emotion"].FirstToUpper();
                    requiredEmotion = (Emotion)Enum.Parse(typeof(Emotion), emotionName);
                }

                if (!data.ContainsKey("Failsafe"))
                {
                    Debug.LogError($"No failsafe on {name}");
                    foreach (var key in data.Keys) Debug.LogError($"---| Available key : {key}");
                    
                    return false;
                }
                else failsafe = data["Failsafe"];

                if (!data.ContainsKey("Childs"))
                {
                    Debug.LogError($"No Childs on {name}");
                    foreach (var key in data.Keys) Debug.LogError($"---| Available key : {key}");
                    
                    return false;
                }
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

        public override bool HasArcEnded => hasArcEnded;
        private bool hasArcEnded;
        
        public override int Branches => currentNode.Childs.Count;

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

        public override void Bootup(RuntimeCharacterBase runtimeCharacter)
        {
            base.Bootup(runtimeCharacter);
            hasArcEnded = false;
            
            rootNodeKeys = new List<string>();
            nodes = new Dictionary<string, DialogueNode>();
            failsafes = new Dictionary<string, DialogueFailsafe>();
            attributes = new HashSet<string>();
        }

        public void ResetArcCompletion() => hasArcEnded = false;
        public void AddAttribute(string attribute) => attributes.Add(attribute);
        
        public override bool IsValid(Emotion emotion, out int selection, out int followingBranches)
        {
            selection = 0;
            followingBranches = 0;
            
            if (currentNode.Childs[0] == "Empty") return false;

            for (var i = 0; i < currentNode.Childs.Count; i++)
            {
                if (!nodes.TryGetValue(currentNode.Childs[i], out var child)) continue;

                if (child.RequiredEmotion == emotion && attributes.IsSupersetOf(child.NeededAttributes))
                {
                    selection = i;
                    
                    if (child.Childs[0] == "Empty") followingBranches = 0;
                    else followingBranches = child.Childs.Count;
                    
                    return true;
                }
            }

            return false;
        }
        public override Dialogue[] GetDialogues(Emotion emotion)
        {
            if (currentNode.Childs[0] == "Empty") return GetDefaultDialogues();
            
            //Debug.Log($"--|DIAG|-> [{Actor}] : {currentNode.Name}");
            for (var i = 0; i < currentNode.Childs.Count; i++)
            {
                if (!nodes.TryGetValue(currentNode.Childs[i], out var child))
                {
                    Debug.LogError($"For current node : {currentNode.Name} | Child {currentNode.Childs[i]} does not exist");
                    continue;
                }

                if (child.RequiredEmotion == emotion && attributes.IsSupersetOf(child.NeededAttributes))
                {
                    foreach (var attribute in child.GivenAttributes) attributes.Add(attribute);
                    currentNode = child;

                    var dialogue = child.GetDialogue();
                    //Debug.Log($"--|DIAG|-> Dialogue found : {dialogue}");

                    if (child.Childs[0] == "Empty")
                    {
                        //Debug.Log($"--|DIAG|-> End of narrative arc for : {name}");
                        hasArcEnded = true;
                        
                        if (GameState.NotifyMusicianArcEnd(out var blockDialogue))
                        {
                            return new Dialogue[]
                            {
                                child.GetDialogue(),
                                blockDialogue
                            };
                        }
                    }
                    else return new Dialogue[] {child.GetDialogue()};
                }
            }

            return GetDefaultDialogues();
        }
        private Dialogue[] GetDefaultDialogues()
        {
            if (failsafes.TryGetValue(currentNode.Failsafe, out var failsafe))
            {
                var failsafeDialogue = failsafe.GetDialogue();
                //Debug.Log($"--|DIAG|-> Falling back to : {failsafe}");
                
                return new Dialogue[] { failsafeDialogue };
            }
            else
            {
                Debug.LogError($"For current node : {currentNode.Name} | Failsafe {currentNode.Failsafe} does not exist");
                //Debug.Log("--|DIAG|-> No correct options : Reverting to current");
                
                return new Dialogue[] { currentNode.GetDialogue() };
            }
        }
        
        protected override void OnBlockPassed()
        {
            if (rootNodeKeys.Count == 0 || rootNodeKeys[GameState.BlockIndex] == "Empty") return;

            currentNode = nodes[rootNodeKeys[GameState.BlockIndex]];
            base.OnBlockPassed();
        }
    }
}