using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Febucci.UI.Core;
using Flux;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = Flux.Event;

namespace BeauTambour
{
    public class DialogueManager : MonoBehaviour
    {
        #region Encapsulated Types

        [Serializable]
        public struct AnchorPoint //Serializable KeyValuePair Struct for runtime dictionary generation
        {
            public Anchor Anchor => anchor;
            public Transform Point => point;
            
            [SerializeField] private Anchor anchor; // Key
            [SerializeField] private Transform point; // Value
        }
        
        public enum EventType
        {
            OnEnd, // Called when all cues have been played$
            OnNext
        }
        #endregion
        
        //--------------------------------------------------------------------------------------------------------------

        public Character SpeakingCharacter => character;
        
        //--------------------------------------------------------------------------------------------------------------
        
        [SerializeField] private ActorCharacterRegistry actorCharacterRegistry; // Enum By ScriptableObject AssetDictionary
        [SerializeField] private AnchorPoint[] anchorPoints;

        [Space, SerializeField] private DialogueBounds bounds; // Bounds used to display text inside
        [SerializeField] private int lineCount = 2; // Max allowed amount of line inside bounds
        [SerializeField] private float heightReduction;
        [SerializeField] private float extra;

        //--------------------------------------------------------------------------------------------------------------
        
        private Cue cue => dialogue[advancement]; // Current Cue

        private List<(string text, int height)> subTexts; // Cue that has been splitted into smaller cues
        private int subAdvancement; // Index of the current small cue
            
        private Dialogue dialogue; // Current Dialogue

        private int advancement; // Index of the current cue
        private Character character; // Character delivering the current cue
        
        private Dictionary<Anchor, Transform> runtimeAnchorPoints;
        
        //--------------------------------------------------------------------------------------------------------------
        
        void Awake()
        {
            advancement = -1;
            subAdvancement = -1;
            
            runtimeAnchorPoints = new Dictionary<Anchor, Transform>();
            foreach (var anchorPoint in anchorPoints) // Dictionary generation
            {
                if (runtimeAnchorPoints.ContainsKey(anchorPoint.Anchor)) continue;
                runtimeAnchorPoints.Add(anchorPoint.Anchor, anchorPoint.Point);
            }
            
            subTexts = new List<(string text, int height)>();
            
            Event.Open(EventType.OnEnd);
            Event.Open<int, string>(EventType.OnNext);
            
            Event.Register(OperationEvent.Skip, Next);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        // Entry point
        public void BeginDialogue(Dialogue dialogue)
        {
            this.dialogue = dialogue;
            bounds.gameObject.SetActive(true);

            Next();
        }
        
        public void Next()
        {
            if (subAdvancement != -1) // If there are small cues, play them
            {
                var tuple = subTexts[subAdvancement]; // Retrieve small cue
                bounds.TextMesh.GetTextInfo(Regex.Replace(tuple.text, "<.\\w+>", string.Empty));
                
                //Actualize bounds
                ResizeBounds();
                PlaceBounds();

                bounds.SetText(tuple.text);
                
                // Update small cue current index
                if (subAdvancement + 1 >= subTexts.Count) subAdvancement = -1;
                else subAdvancement++;
            }
            else
            {
                advancement++; // Update cue index
                if (advancement >= dialogue.Length) // Exit point
                {
                    EndDialogue();
                    return;
                }
                
                character = actorCharacterRegistry[cue.Actor]; // Retrieve associated character

                // Setup text data
                bounds.TextMesh.font = character.Font;
                bounds.TextMesh.color = character.FontColor;
                bounds.color = character.BackgroundColor;
                
                // Reboot bounds & text for correct actualization
                bounds.Reboot();
                bounds.TextMesh.ForceMeshUpdate();
                
                var info = bounds.TextMesh.GetTextInfo(Regex.Replace(cue.Text, "\\<(.*?)\\>", string.Empty));
                if (info.lineCount > lineCount) // Cue is too large, split it into smaller cues
                {
                    // Prepare small cues container
                    subAdvancement = 0;
                    subTexts.Clear();
                    
                    // loop data
                    var previousIndex = 0;
                    var index = lineCount - 1;
                    var prolong = true;
                    
                    while (prolong) // Small cue identification & caching
                    {
                        if (index >= info.lineCount)
                        {
                            index = info.lineCount - 1;
                            previousIndex = Mathf.Clamp(previousIndex, 0, index);
                            
                            prolong = false;
                        }

                        var firstIndex = info.lineInfo[previousIndex].firstCharacterIndex;
                        var lastCharacterIndex = info.lineInfo[index].lastCharacterIndex;

                        var height = index - previousIndex;
                        var text = cue.Text.Substring(firstIndex, lastCharacterIndex - firstIndex); // Actual small cue
                            
                        subTexts.Add((text, height));

                        previousIndex = index + 1;
                        index += lineCount;
                    }
                    
                    Next(); // Skip to directly play first small cue
                }
                else
                {
                    // Actualize Bounds
                    ResizeBounds();
                    PlaceBounds();
                    
                    bounds.SetText(cue.Text);
                }
                
                Event.Call<int, string>(EventType.OnNext, advancement, cue.Text);
            }
        }
        
        // ExitPoint
        private void EndDialogue()
        {
            advancement = -1;
            bounds.gameObject.SetActive(false);
            
            Event.Call(EventType.OnEnd);
        }

        //--------------------------------------------------------------------------------------------------------------
        
        private void PlaceBounds()
        {
            var offset = Vector2.left * (bounds.Width / 2f) + Vector2.up * 0.2f;
            var position = (Vector2)character.Instance.DialoguePoint.position + offset;
            if (character.Anchor == Anchor.Right) 
            {                
                bounds.Place(position, character.Instance.DialoguePoint.position, Vector2.right, true);
            }
            else if (character.Anchor == Anchor.Left)
            {
                bounds.Place(position, character.Instance.DialoguePoint.position, Vector2.left, true);
            }
            else
            {
                Debug.LogError($"Undefined anchor for dialogue display : {character.Anchor} at {character}");
                Debug.Break();
            }
        }
        private void ResizeBounds()
        {
            // Necessary to force visual update
            bounds.TextMesh.enabled = false;
            bounds.TextMesh.enabled = true;
            
            var height = bounds.TextMesh.textInfo.lineInfo.First().lineHeight * bounds.TextMesh.textInfo.lineCount;
            height += bounds.TextMesh.margin.y + bounds.TextMesh.margin.w;
            height -= heightReduction * bounds.TextMesh.textInfo.lineCount;
            height -= extra * (bounds.TextMesh.textInfo.lineCount - 1);
            
            var maximumWidth = bounds.TextMesh.textInfo.lineInfo.Max(line => line.maxAdvance);
            maximumWidth += bounds.TextMesh.margin.z;

            bounds.Resize(new Vector2(maximumWidth, height));
        }
    }
}