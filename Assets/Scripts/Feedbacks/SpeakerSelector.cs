using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Spine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerSelector : MonoBehaviour
    {
        [SerializeField] private GameObject arcInProgressVisual;
        [SerializeField] private CopyMesh outline;

        void Awake() => Event.Register<ISpeaker>(GameEvents.OnSpeakerSelected, OnSpeakerSelected);

        void OnSpeakerSelected(ISpeaker speaker) 
        { 
            transform.position = speaker.RuntimeLink.TopCenter;
            arcInProgressVisual.SetActive(!speaker.IsArcEnded);

            Vector3 speakerPos = speaker.RuntimeLink.transform.position;
            outline.transform.position = new Vector3(speakerPos.x,speakerPos.y+0.01f);
            outline.CopyFrom = speaker.RuntimeLink.GetComponentInChildren<MeshFilter>();
        } 
    }
}