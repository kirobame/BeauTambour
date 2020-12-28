using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SpeakerSelector : MonoBehaviour
    {
        void Awake() => Event.Register<ISpeaker>(GameEvents.OnSpeakerSelected, OnSpeakerSelected);

        void OnSpeakerSelected(ISpeaker speaker)
        {
            Debug.Log("Speaker selection !");
            transform.position = speaker.RuntimeLink.TopCenter;
        }
    }
}