using System;
using System.Collections.Generic;
using Febucci.UI;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class DialogueSignalHandler : MonoBehaviour
    {
        [SerializeField] private SignalCollection signalCollection;
        
        [Space, SerializeField] private TextAnimator textAnimator;
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;

        private Queue<System.Collections.Generic.KeyValuePair<Signal, string[]>> currentSignals;
        
        void Awake()
        {
            currentSignals = new Queue<System.Collections.Generic.KeyValuePair<Signal, string[]>>();
            signalCollection.Initialize();
        }

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
            var charArray = emotionName.ToCharArray();
            charArray[0] = Char.ToUpper(charArray[0]);
            emotionName = new string(charArray);
            
            var emotion = (Emotion)Enum.Parse(typeof(Emotion), emotionName);
            
            var clarity = int.Parse(mainArgs[1]);
            var await = bool.Parse(mainArgs[2]);

            if (!signalCollection.TrySelect(category, emotion, clarity, out var signal)) return;
            Debug.Log($"FOR : {message} --> PLAYING SIGNAL : {signal}");
            
            var subArgs = split[2].Split(',');
            if (await)
            {
                var kvp = new System.Collections.Generic.KeyValuePair<Signal, string[]>(signal, subArgs);
                currentSignals.Enqueue(kvp);
                if (currentSignals.Count > 1) return;
                
                signal.OnEnd += OnSignalEnd;

                Debug.Log("1 - TAnimPlayer : " + textAnimatorPlayer);
                textAnimatorPlayer.StopShowingText();
            }

            var dialogueManager = Repository.GetSingle<DialogueManager>(Reference.DialogueManager);
            signal.Execute(this, dialogueManager.SpeakingCharacter, subArgs);
        }

        void OnSignalEnd()
        {
            var kvp = currentSignals.Dequeue();
            kvp.Key.OnEnd -= OnSignalEnd;
            
            if (currentSignals.Count > 0)
            {
                var newKvp = currentSignals.Peek();
                newKvp.Key.OnEnd += OnSignalEnd;
                
                var dialogueManager = Repository.GetSingle<DialogueManager>(Reference.DialogueManager);
                newKvp.Key.Execute(this, dialogueManager.SpeakingCharacter, newKvp.Value);
            }
            else
            {
                Debug.Log("2 - TAnimPlayer : " + textAnimatorPlayer);
                textAnimatorPlayer.StartShowingText();
            }
        }
    }
}