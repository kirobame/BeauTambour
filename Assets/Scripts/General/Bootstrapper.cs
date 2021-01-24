using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Shapes;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Encounter encounter;
        [SerializeField] private bool useBackup;
        [SerializeField] private bool playIntro;
        [SerializeField] private bool skipStart;

        [Space, SerializeField] private int startingBlock;

        void Awake()
        {
            if (BootstrappingRelay.StartingBlock != -1) GameState.Bootup(BootstrappingRelay.StartingBlock - 1);
            else GameState.Bootup(startingBlock - 1);

            if (BootstrappingRelay.UseBackup != -1) useBackup = BootstrappingRelay.UseBackup != 0;
            if (BootstrappingRelay.PlayIntro != -1) playIntro = BootstrappingRelay.PlayIntro != 0; 
            if (BootstrappingRelay.SkipStart != -1) skipStart = BootstrappingRelay.SkipStart != 0;
        }
        void Start()
        {
            if (!playIntro && !useBackup)
            {
                Event.Open(ExtraEvents.OnDownloadOnlyConfirmed);
                Event.Call(ExtraEvents.OnDownloadOnlyConfirmed);
                
                Event.Open(ExtraEvents.OnDownloadOnly);
                Event.Register(GameEvents.OnEncounterBootedUp, () => Event.Call(ExtraEvents.OnDownloadOnly));
                    
                Event.Open(GameEvents.OnIntroEnd);
                Event.Register(GameEvents.OnIntroEnd, PlayFirstPhase);
            }
            else if (!playIntro && useBackup)
            {
                //Debug.Log("NOT PLAYING INTRO");
                Event.Open(ExtraEvents.OnIntroSkipped);
                Event.Call(ExtraEvents.OnIntroSkipped);
                
                Event.Register(GameEvents.OnEncounterBootedUp, PlayFirstPhase);
            }
            else
            {
                //Debug.Log("PLAYING INTRO");

                if (!useBackup)
                {
                    Event.Open(ExtraEvents.OnRegularDownload);
                    Event.Call(ExtraEvents.OnRegularDownload);
                    
                    Event.Open(ExtraEvents.OnRegularDownloadEnd);
                }
                
                Event.Open(GameEvents.OnIntroConfirmed);
                Event.Call(GameEvents.OnIntroConfirmed);
                
                Event.Open(GameEvents.OnIntroStart);
                Event.Open(GameEvents.OnIntroEnd);

                Event.Register(GameEvents.OnEncounterBootedUp, OnEncounterBootedUp);
                Event.Register(GameEvents.OnIntroEnd, PlayFirstPhase);
            }
            
            StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            encounter.Bootup(this, useBackup);
        }

        void OnEncounterBootedUp() => StartCoroutine(EncounterBootupRoutine());
        private IEnumerator EncounterBootupRoutine()
        {
            if (!useBackup)
            {
                Event.Call(ExtraEvents.OnRegularDownloadEnd);
                yield return new WaitForSeconds(1.0f);
            }
            
            Event.Call(GameEvents.OnIntroStart);
        }
        
        void PlayFirstPhase()
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            if (!playIntro) Event.Call(GameEvents.OnCurtainRaised);

            if (skipStart)
            {
                //Debug.Log("SKIPPING START");
                
                var dialoguePhase = phaseHandler.Get<DialoguePhase>(PhaseCategory.Dialogue);
                dialoguePhase.SkipBootUp();
                
                phaseHandler.Play(PhaseCategory.SpeakerSelection);
            }
            else
            {
                //Debug.Log("NOT SKIPPING START");
                phaseHandler.Play(PhaseCategory.Dialogue);
            }
        }
    }
}