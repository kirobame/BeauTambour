using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;
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

        void Awake() => GameState.Bootup(startingBlock - 1);
        void Start()
        {
            if (!playIntro)
            {
                Debug.Log("NOT PLAYING INTRO");
                Event.Register(GameEvents.OnEncounterBootedUp, PlayFirstPhase);
            }
            else
            {
                Debug.Log("PLAYING INTRO");
                
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

        void OnEncounterBootedUp() => Event.Call(GameEvents.OnIntroStart);
        void PlayFirstPhase()
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);

            if (skipStart)
            {
                Debug.Log("SKIPPING START");
                
                var dialoguePhase = phaseHandler.Get<DialoguePhase>(PhaseCategory.Dialogue);
                dialoguePhase.SkipBootUp();
                
                phaseHandler.Play(PhaseCategory.SpeakerSelection);
            }
            else
            {
                Debug.Log("NOT SKIPPING START");
                phaseHandler.Play(PhaseCategory.Dialogue);
            }
        }
    }
}