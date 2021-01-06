using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class SignalHandler : MonoBehaviour
    {
        [SerializeField] private SignalCollection signalCollection;
        
        [Space, SerializeField] private TextAnimator textAnimator;
        [SerializeField] private TextAnimatorPlayer textAnimatorPlayer;

        [Space, SerializeField] private Animator indicator;
        private bool awaitingIndication;

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

            message = message.Replace(" ", string.Empty);
            var trimmedMessage = message.Substring(2, message.Length - 2);
            var split = trimmedMessage.Split('/');

            if (split.Length != 3)
            {
                Debug.LogWarning($"Signal parsing error -:- There must be 3 parts in a signal -||- ({message})");
                return;
            }

            var category = split[0];
            var mainArgs = split[1].Split(',');

            if (mainArgs.Length != 3)
            {
                Debug.LogWarning($"Signal parsing error -:- There must be 3 subargs in the 2nd part -||- ({message})");
                return;
            }

            if (!Enum.TryParse<Emotion>(mainArgs[0].FirstToUpper(), out var emotion))
            {
                Debug.LogWarning($"Signal parsing error -:- The emotion argument could not be parsed -||- ({message})");
                return;
            }
            if (!Enum.TryParse<Actor>(mainArgs[1].FirstToUpper(), out var actor))
            {
                Debug.LogWarning($"Signal parsing error -:- The actor argument could not be parsed -||- ({message})");
                return;
            }

            var speaker = Extensions.GetCharacter<Character>(actor) as ISpeaker;
            if (speaker == null)
            {
                Debug.LogWarning($"Signal error -:- There is no corresponding character -||- ({message})");
                return;
            }

            if (!bool.TryParse(mainArgs[2], out var await))
            {
                Debug.LogWarning($"Signal parsing error -:- Await argument could not be parsed -||- ({message})");
                return;
            }
            if (!signalCollection.TrySelect(category, emotion, out var signal))
            {
                Debug.LogWarning($"Signal error -:- No signal found for the given arguments -||- ({message})");
                return;
            }

            var subArgs = split[2].Split(',');
            if (await)
            {
                var kvp = new KeyValuePair<Signal, string[]>(signal, subArgs);
                queuedSignals.Enqueue(kvp);
                
                if (queuedSignals.Count > 1) return;
                
                BeginIndication(speaker);
                
                signal.OnEnd += OnSignalEnd;
                textAnimatorPlayer.StopShowingText();
            }
            
            signal.Execute(this, speaker, subArgs);
        }

        private void BeginIndication(ISpeaker speaker)
        {
            if (!indicator.GetCurrentAnimatorStateInfo(0).IsTag("Void")) return;

            speaker.StopTalking();
            if (textAnimator.latestCharacterShown.index + 2 < textAnimator.tmproText.textInfo.characterCount)
            {
                awaitingIndication = true;

                var character = textAnimator.tmproText.textInfo.characterInfo[textAnimator.latestCharacterShown.index + 2];
                var position = textAnimator.transform.TransformPoint(character.bottomLeft);
                indicator.transform.parent.position = position;

                indicator.SetTrigger("In");
            }
            else awaitingIndication = false;
        }
        private void EndIndication() => indicator.SetTrigger("Out");

        private IEnumerator EndRoutine(ISpeaker speaker)
        {
            if (awaitingIndication)
            {
                EndIndication();
                yield return new WaitUntil(() => indicator.GetCurrentAnimatorStateInfo(0).IsTag("Void"));
                
                speaker.BeginTalking();
                textAnimatorPlayer.StartShowingText();
            }
            else
            {
                speaker.StopTalking();
                Event.Call(GameEvents.OnCueFinished);
            }
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