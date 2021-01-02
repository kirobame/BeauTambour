using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerSelector : MonoBehaviour
    {
        public GameObject arcInProgressVisual;

        void Awake() => Event.Register<ISpeaker>(GameEvents.OnSpeakerSelected, OnSpeakerSelected);

        void OnSpeakerSelected(ISpeaker speaker) 
        { 
            transform.position = speaker.RuntimeLink.TopCenter;
            arcInProgressVisual.SetActive(!speaker.IsArcEnded);            
        } 
    }
}