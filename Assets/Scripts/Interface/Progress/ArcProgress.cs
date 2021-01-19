﻿using System.Collections;
using System.Linq;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class ArcProgress : MonoBehaviour
    {
        [SerializeField] private ArcSegment[] segments;

        private int speakerCount;
        
        void Awake()
        {
            Event.Register(GameEvents.OnGoingToNextBlock, OnGoingToNextBlock);
            
            Event.Open<Musician>(GameEvents.OnMusicianArcCompleted);
            Event.Register<Musician>(GameEvents.OnMusicianArcCompleted, OnMusicianArcCompleted);
        }

        void OnGoingToNextBlock()
        {
            speakerCount = GameState.ActiveSpeakers.Count() - 1;
            for (var i = 0; i < speakerCount; i++)
            {
                segments[i].gameObject.SetActive(true);
                segments[i].Reboot();
            }

            for (var i = speakerCount; i < segments.Length; i++) segments[i].gameObject.SetActive(false);
        }

        void OnMusicianArcCompleted(Musician musician)
        {
            var index = speakerCount - GameState.FinishedArcsCount - 1;
            if (index < 0) return;
            
            segments[index].Complete(musician);
        }
    }
}