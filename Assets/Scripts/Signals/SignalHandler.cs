using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class SignalHandler : MonoBehaviour
    {
        [SerializeField] private SignalCollection signalCollection;
        
        [Space, SerializeField] private TextAnimator textAnimator;
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;

        [Space, SerializeField] private Animator indicator;

        private Queue<KeyValuePair<Signal, string[]>> queuedSignals;
        
        void Awake()
        {
            queuedSignals = new Queue<KeyValuePair<Signal, string[]>>();
            signalCollection.BootUp();
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
            var await = bool.Parse(mainArgs[1]);

            if (!signalCollection.TrySelect(category, emotion, out var signal)) return;

            var subArgs = split[2].Split(',');
            var dialogueManager = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            
            if (await)
            {
                var kvp = new KeyValuePair<Signal, string[]>(signal, subArgs);
                queuedSignals.Enqueue(kvp);
                
                if (queuedSignals.Count > 1) return;
                
                BeginIndication(dialogueManager.Speaker);
                
                signal.OnEnd += OnSignalEnd;
                textAnimatorPlayer.StopShowingText();
            }
            
            signal.Execute(this, dialogueManager.Speaker, subArgs);
        }

        private void BeginIndication(ISpeaker speaker)
        {
            if (!indicator.GetCurrentAnimatorStateInfo(0).IsTag("Void")) return;

            speaker.StopTalking();
            
            var character = textAnimator.tmproText.textInfo.characterInfo[textAnimator.latestCharacterShown.index + 2];
            var position = textAnimator.transform.TransformPoint(character.bottomLeft);
            indicator.transform.parent.position = position;
            
            indicator.SetTrigger("In");
        }
        private void EndIndication() => indicator.SetTrigger("Out");

        private IEnumerator EndRoutine(ISpeaker speaker)
        {
            EndIndication();
            yield return new WaitUntil(() => indicator.GetCurrentAnimatorStateInfo(0).IsTag("Void"));
            
            speaker.BeginTalking();
            textAnimatorPlayer.StartShowingText();
        }
        
        void OnSignalEnd()
        {
            var dialogueManager = Repository.GetSingle<DialogueHandler>(References.DialogueHandler);
            
            var kvp = queuedSignals.Dequeue();
            kvp.Key.OnEnd -= OnSignalEnd;

            if (queuedSignals.Count > 0)
            {
                BeginIndication(dialogueManager.Speaker);

                var newKvp = queuedSignals.Peek();
                newKvp.Key.OnEnd += OnSignalEnd;
                
                newKvp.Key.Execute(this, dialogueManager.Speaker, newKvp.Value);
            }
            else StartCoroutine(EndRoutine(dialogueManager.Speaker));
        }
    }
}