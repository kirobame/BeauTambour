using System;
using System.Collections;
using Febucci.UI;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class TypewriterAnalyzer : MonoBehaviour
    {
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
        [SerializeField] private int gap;

        private DialogueHandler dialogueHandler;
        private int counter;

        void Start()
        {
            dialogueHandler = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            Event.Open(GameEvents.OnCueFinished);
        }

        void OnEnable()
        {
            counter = 0;
            
            textAnimatorPlayer.onCharacterVisible.AddListener(OnCharacterVisible);
            textAnimatorPlayer.onTextShowed.AddListener(OnTextShown);
        }
        void OnDisable()
        {
            textAnimatorPlayer.onCharacterVisible.RemoveListener(OnCharacterVisible);
            textAnimatorPlayer.onTextShowed.RemoveListener(OnTextShown);
        }

        void OnTextShown()
        {
            dialogueHandler.Speaker.StopTalking();
            Event.Call(GameEvents.OnCueFinished);
        }
        void OnCharacterVisible(char character)
        {
            if (counter <= 0)
            {
                character = char.ToLower(character);
                if (dialogueHandler.Speaker.AudioCharMap.TryGet(character, out var package))
                {
                    var audioPool = Repository.GetSingle<AudioPool>(References.AudioPool);
                    var audio = audioPool.RequestSingle();
                    
                    package.AssignTo(audio, EventArgs.Empty);
                    audio.Play();

                    counter = gap;
                }
            }
            else counter--;
        }
    }
}