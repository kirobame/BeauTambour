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
        [SerializeField] private GameObject arcInProgressVisual;
        [SerializeField] private CopyMesh outline;

        [Space, SerializeField] private PoolableAnimation selectionEffect;

        private ISpeaker speaker;

        void Awake()
        {
            Event.Register<ISpeaker>(GameEvents.OnSpeakerSelected, OnSpeakerSelected);
            Event.Register(GameEvents.OnSpeakerChoice, OnSpeakerChoice);
        }

        void OnSpeakerSelected(ISpeaker speaker)
        {
            this.speaker = speaker;
            
            transform.position = speaker.RuntimeLink.TopCenter;
            arcInProgressVisual.SetActive(!speaker.IsArcEnded);

            Vector3 speakerPos = speaker.RuntimeLink.transform.position;
            outline.transform.position = new Vector3(speakerPos.x,speakerPos.y+0.01f);
            outline.CopyFrom = speaker.RuntimeLink.GetComponentInChildren<MeshFilter>();
        }

        void OnSpeakerChoice()
        {
            var animationPool = Repository.GetSingle<AnimationPool>(References.AnimationPool);
            var animator = animationPool.RequestSingle(selectionEffect);

            animator.transform.parent = speaker.RuntimeLink.HeadSocket.Attach;
            animator.transform.localPosition = Vector3.zero;
            
            animator.SetTrigger("In");
        }
    }
}