﻿using System.Collections;
using System.Collections.Generic;
using Flux;
using TMPro;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public abstract class Character : ScriptableObject
    {
        public int Id => GetInstanceID();
        
        public abstract bool HasArcEnded { get; }
        public abstract int Branches { get; }

        public bool isActive;
        
        public Actor Actor => actor;
        [SerializeField] private Actor actor;

        [Space, SerializeField] private Color fontColor;
        [SerializeField] private Color backgroundColor;
        [SerializeField] private TMP_FontAsset font;

        public AudioCharMapPackage AudioCharMap => audioCharMap;
        [Space, SerializeField] private AudioCharMapPackage audioCharMap;

        public RuntimeCharacterBase RuntimeLink => runtimeLink;
        private RuntimeCharacterBase runtimeLink;

        private bool hasActed;
        
        public virtual void Bootup(RuntimeCharacterBase runtimeCharacter)
        {
            hasActed = false;
            
            runtimeLink = runtimeCharacter;
            Repository.Reference(this, References.Characters);

            Event.Register(GameEvents.OnBlockPassed, OnBlockPassed);
        }

        public abstract bool IsValid(Emotion emotion, out int selection, out int followingBranches);
        public abstract Dialogue[] GetDialogues(Emotion emotion);
        
        public virtual void SetupDialogueHolder(DialogueHolder dialogueHolder)
        {
            dialogueHolder.color = backgroundColor;
            
            dialogueHolder.TextMesh.color = fontColor;
            dialogueHolder.TextMesh.font = font;
        }
        public Vector2 GetPositionForDialogueHolder() => runtimeLink.DialogueAnchor;

        protected virtual void OnBlockPassed()
        {
            if (!hasActed)
            {
                Event.Call<Character>(GameEvents.OnSpeakerEntrance, this);
                hasActed = true;
            }
        }
    }
}