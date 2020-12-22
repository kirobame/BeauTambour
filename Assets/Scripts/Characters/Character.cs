using System.Collections;
using System.Collections.Generic;
using Flux;
using TMPro;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public abstract class Character : ScriptableObject
    {
        public Actor Actor => actor;
        [SerializeField] private Actor actor;

        [Space, SerializeField] private Color fontColor;
        [SerializeField] private Color backgroundColor;
        [SerializeField] private TMP_FontAsset font;
        
        public abstract RuntimeCharacter RuntimeLink { get; }

        public virtual void Inject(RuntimeCharacter runtimeCharacter) => Repository.Reference(this, References.Characters);

        public virtual void SetupDialogueHolder(DialogueHolder dialogueHolder)
        {
            dialogueHolder.color = backgroundColor;
            
            dialogueHolder.TextMesh.color = fontColor;
            dialogueHolder.TextMesh.font = font;
        }
        public Vector2 GetPositionForDialogueHolder() => RuntimeLink.transform.position;
    }
}