using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public struct AnchorPoint
        {
            public Anchor Anchor => anchor;
            public Transform Point => point;
            
            [SerializeField] private Anchor anchor;
            [SerializeField] private Transform point;
        }
        
        public enum EventType
        {
            OnEnd,
        }
        #endregion
        
        [SerializeField] private ActorCharacterRegistry actorCharacterRegistry;
        [SerializeField] private AnchorPoint[] anchorPoints;

        [Space, SerializeField] private DialogueBounds bounds;
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private int lineCount = 2;

        private Cue cue => dialogue[advancement];

        private List<(string text, int height)> subTexts;
        private int subAdvancement;
            
        private Dialogue dialogue;

        private int advancement;
        private Character character;
        
        private Dictionary<Anchor, Transform> runtimeAnchorPoints;
        
        void Awake()
        {
            advancement = -1;
            subAdvancement = -1;
            
            runtimeAnchorPoints = new Dictionary<Anchor, Transform>();
            foreach (var anchorPoint in anchorPoints)
            {
                if (runtimeAnchorPoints.ContainsKey(anchorPoint.Anchor)) continue;
                runtimeAnchorPoints.Add(anchorPoint.Anchor, anchorPoint.Point);
            }
            
            subTexts = new List<(string text, int height)>();
            
            Event.Open(EventType.OnEnd);
            Event.Register(OperationEvent.Skip, Next);
        }
        
        public void BeginDialogue(Dialogue dialogue)
        {
            this.dialogue = dialogue;
            bounds.gameObject.SetActive(true);
            
            Next();
        }
        
        public void Next()
        {
            if (subAdvancement != -1)
            {
                var tuple = subTexts[subAdvancement];
                textMesh.text = tuple.text;
                
                ResizeBounds();
                PlaceBounds();

                if (subAdvancement + 1 >= subTexts.Count) subAdvancement = -1;
                else subAdvancement++;
            }
            else
            {
                advancement++;
                if (advancement >= dialogue.Length)
                {
                    EndDialogue();
                    return;
                }
                
                character = actorCharacterRegistry[cue.Actor];
                
                textMesh.font = character.Font;
                textMesh.color = character.FontColor;
                bounds.color = character.BackgroundColor;
                
                bounds.Reboot();
                textMesh.ForceMeshUpdate();
                
                var info = textMesh.GetTextInfo(cue.Text);
                if (info.lineCount > lineCount)
                {
                    subAdvancement = 0;
                    subTexts.Clear();
                    
                    var previousIndex = 0;
                    var index = lineCount - 1;
                    var prolong = true;
                    
                    while (prolong)
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
                        var text = cue.Text.Substring(firstIndex, lastCharacterIndex - firstIndex);
                            
                        subTexts.Add((text, height));

                        previousIndex = index + 1;
                        index += lineCount;
                    }
                    
                    Next();
                }
                else
                {
                    textMesh.text = cue.Text;
                    ResizeBounds();
                    PlaceBounds();
                }
            }
        }

        private void PlaceBounds()
        {
            if (character.Anchor == Anchor.Right)
            {
                var offset = Vector2.left * bounds.Width;
                var position = (Vector2)runtimeAnchorPoints[Anchor.Right].position + offset;
                
                bounds.Place(position, character.Instance.DialoguePoint.position, Vector2.left);
            }
            else if (character.Anchor == Anchor.Left)
            {
                var position = runtimeAnchorPoints[Anchor.Left].position;
                bounds.Place(position, character.Instance.DialoguePoint.position, Vector2.right);
            }
            else
            {
                Debug.LogError($"Undefined anchor for dialogue display : {character.Anchor} at {character}");
                Debug.Break();
            }
            
           
        }
        private void ResizeBounds()
        {
            textMesh.enabled = false;
            textMesh.enabled = true;
            
            var height = textMesh.textInfo.lineInfo.First().lineHeight * textMesh.textInfo.lineCount;
            height += textMesh.margin.y + textMesh.margin.w;
            
            var maximumWidth = textMesh.textInfo.lineInfo.Max(line => line.maxAdvance);
            maximumWidth += textMesh.margin.z;
            
            bounds.Resize(new Vector2(maximumWidth, height));
        }

        private void EndDialogue()
        {
            advancement = -1;
            bounds.gameObject.SetActive(false);
            
            Event.Call(EventType.OnEnd);
        }
    }
}