using System;
using Febucci.UI;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class LetterAudioPlayer : MonoBehaviour
    {
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;
        [SerializeField] private int gap;

        private DialogueManager dialogueManager;
        private int counter;

        void Awake()
        {
            dialogueManager = Repository.GetSingle<DialogueManager>(Reference.DialogueManager);
            Event.Register<int,string>(DialogueManager.EventType.OnNext, OnNewCue);
        }
        
        void OnEnable()
        {
            counter = 0;
            textAnimatorPlayer.onCharacterVisible.AddListener(OnCharacterVisible);
        }
        void OnDisable() => textAnimatorPlayer.onCharacterVisible.RemoveListener(OnCharacterVisible);

        private void OnCharacterVisible(char character)
        {
            if (counter <= 0)
            {
                var letterAudioRegistry = dialogueManager.SpeakingCharacter.LetterAudioRegistry;
                character = char.ToLower(character);

                if (letterAudioRegistry.TryGet(character, out var clip))
                {
                    var audioPool = Repository.GetSingle<AudioPool>(Pool.Audio);
                    var audio = audioPool.RequestSingle();
                
                    audio.clip = letterAudioRegistry[character];
                    audio.Play();
                
                    counter = gap;
                }
            }
            else counter--;
        }

        private void OnNewCue(int index, string text) => counter = 0;
    }
}