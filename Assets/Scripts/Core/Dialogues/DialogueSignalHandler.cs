using System;
using Febucci.UI;
using Flux;
using Ludiq.PeekCore;
using UnityEngine;

namespace BeauTambour
{
    public class DialogueSignalHandler : MonoBehaviour
    {
        [SerializeField] private SignalCollection signalCollection;
        
        [Space, SerializeField] private TextAnimator textAnimator;
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;

        private Signal currentSignal;
        
        void Awake() => signalCollection.Initialize();
        
        void OnEnable() => textAnimator.onEvent += HandleEvent;
        void OnDisable() => textAnimator.onEvent -= HandleEvent;

        void HandleEvent(string message)
        {
            if (message.Substring(0, 2) != "e:") return;

            var trimmedMessage = message.Substring(2, message.Length - 2);
            var split = trimmedMessage.Split('/');

            var category = split[0];
            var mainArgs = split[1].Split(',');

            var emotionName = mainArgs[0];
            emotionName = emotionName.FirstCharacterToUpper();
            var emotion = (Emotion)Enum.Parse(typeof(Emotion), emotionName);
            
            var clarity = int.Parse(mainArgs[1]);
            var await = bool.Parse(mainArgs[2]);

            currentSignal = signalCollection.Select(category, emotion, clarity);
            if (await)
            {
                currentSignal.OnEnd += OnSignalEnd;
                textAnimatorPlayer.StopShowingText();
            }

            var dialogueManager = Repository.GetSingle<DialogueManager>(Reference.DialogueManager);
            var subArgs = split[2].Split(',');
            
            currentSignal.Execute(this, dialogueManager.SpeakingCharacter, subArgs);
        }

        void OnSignalEnd()
        {
            currentSignal.OnEnd -= OnSignalEnd;
            textAnimatorPlayer.StartShowingText();
        }
    }
}