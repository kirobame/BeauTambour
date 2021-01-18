using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;
using Utilities.Spine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerSelector : MonoBehaviour
    {
        [SerializeField] private CopyMesh outline;

        [Space, SerializeField] private PoolableAnimation selectionEffect;
        [SerializeField] private SpriteRenderer leftArrow, rightArrow;

        private Character speaker;

        void Awake()
        {
            Event.Register<Character, int>(GameEvents.OnSpeakerSelected, OnSpeakerSelected);
            Event.Register(GameEvents.OnSpeakerChoice, OnSpeakerChoice);
        }

        void OnSpeakerSelected(Character speaker, int code)
        {
            this.speaker = speaker;

            if (code == 0)
            {
                leftArrow.enabled = true;
                rightArrow.enabled = true;
            }
            else if (code == 1)
            {
                leftArrow.enabled = true;
                rightArrow.enabled = false;
            }
            else if (code == 2)
            {
                leftArrow.enabled = false;
                rightArrow.enabled = true;
            }
            
            transform.position = speaker.RuntimeLink.SelectionAnchor;
            
            Vector3 speakerPos = speaker.RuntimeLink.transform.position;
            outline.transform.position = new Vector3(speakerPos.x,speakerPos.y, -0.15f);

            var scale = speaker.RuntimeLink.Animator.transform.localScale;
            if (speaker is Interlocutor) outline.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
            else outline.transform.localScale = scale;
            
            var lookup = speaker.RuntimeLink.GetComponentInChildren<OutlineLookup>();
            if (lookup == null) outline.gameObject.SetActive(false);
            else
            {
                outline.gameObject.SetActive(true);
                
                outline.CopyFrom = lookup.Filter;
                outline.SetMaterial(lookup.Large);
            }
        }

        void OnSpeakerChoice()
        {
            leftArrow.enabled = false;
            rightArrow.enabled = false;
            
            var animationPool = Repository.GetSingle<AnimationPool>(References.AnimationPool);
            var animator = animationPool.RequestSingle(selectionEffect);

            animator.transform.parent = speaker.RuntimeLink.HeadSocket.Value;
            animator.transform.localPosition = Vector3.zero;
            animator.transform.localScale = Vector3.one;
            
            animator.SetTrigger("In");
        }
    }
}