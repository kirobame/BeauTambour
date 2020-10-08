using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class RythmHandler : MonoBehaviour
    {
        public const double StandardErrorMargin = 0.175d;
        
        public event Action<double> OnBeat;
        
        public bool IsActive => audioSource.isPlaying;
        
        public double SecondsPerBeats => 60f / beatsPerMinutes;
        public int BeatsPerMinutes => beatsPerMinutes;
        
        public double Time { get; private set; }
        public double Beats { get; private set; }
        
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private int beatsPerMinutes;

        private double startTime;
        private int lastBeat = -1;

        private List<(Action action, int countdown)> plainTimedActions = new List<(Action action, int countdown)>();
        private List<(Action<double> action, double countdown)> timedActions = new List<(Action<double> action, double countdown)>();
        
        [Button]
        public void Calibrate()
        {
            startTime = AudioSettings.dspTime;
            audioSource.Play();
        }
        [Button]
        public void Stop()
        {
            audioSource.Stop();

            Time = 0d;
            Beats = 0d;
            
            lastBeat = -1;
        }

        void Update()
        {
            if (!IsActive) return;

            Time = AudioSettings.dspTime - startTime;
            
            var beats = Time / SecondsPerBeats;
            var difference = beats - Beats;
            Beats = beats;

            Tick(difference);
            
            var roundedBeats = (int)Math.Floor(Beats);
            if (roundedBeats != lastBeat)
            {
                PlainTick();
                
                lastBeat = roundedBeats;
                OnBeat?.Invoke(lastBeat);
            }
        }

        private void Tick(double difference)
        {
            var index = 0;
            while (index < timedActions.Count)
            {
                var removalIndex = -1;
                
                for (var i = index; i < timedActions.Count; i++)
                {
                    if (timedActions[i].countdown <= 0)
                    {
                        removalIndex = i;
                        break;
                    }
                    
                    var timedAction = timedActions[i];

                    if (timedAction.countdown <= 1) timedAction.action(MathPt.Clamp01(Beats - lastBeat));
                    timedAction.countdown -= difference;

                    timedActions[i] = timedAction;
                    index++;
                }
                
                if (removalIndex != -1) timedActions.RemoveAt(removalIndex);
            }
        }
        private void PlainTick()
        {
            var index = 0;
            while (index < plainTimedActions.Count)
            {
                var removalIndex = -1;
                
                for (var i = index; i < plainTimedActions.Count; i++)
                {
                    if (plainTimedActions[i].countdown <= 0)
                    {
                        plainTimedActions[i].action();
                        
                        removalIndex = i;
                        break;
                    }
                    
                    var plainTimedAction = plainTimedActions[i];
                    plainTimedAction.countdown--;

                    plainTimedActions[i] = plainTimedAction;
                    index++;
                }
                
                if (removalIndex != -1) plainTimedActions.RemoveAt(removalIndex);
            }
        }

        public bool TryEnqueue(Action action, double errorMargin = StandardErrorMargin)
        {
            if (action == null || !IsActive || errorMargin <= 0 || errorMargin > SecondsPerBeats) return false;
            var code = IsInputValid(errorMargin, out var flooredDifference, out var ceiledDifference);

            if (code == 1)
            {
                plainTimedActions.Add((action, 0));
                return true;
            }
            else if (code == 2)
            {
                plainTimedActions.Add((action, 1));
                return true;
            }

            return false;
        }
        public bool TryEnqueue(Action<double> action, double errorMargin = StandardErrorMargin)
        {
            if (action == null || !IsActive || errorMargin <= 0 || errorMargin > SecondsPerBeats) return false;
            var code = IsInputValid(errorMargin, out var flooredDifference, out var ceiledDifference);

            if (code == 1)
            {
                var countdown = 1d - flooredDifference;
                timedActions.Add((action, countdown));

                return true;
            }
            else if (code == 2)
            {
                var countdown = 1d + ceiledDifference;
                timedActions.Add((action, countdown));
                
                return true;
            }

            return false;
        }

        private int IsInputValid(double errorMargin, out double flooredDifference, out double ceiledDifference)
        {
            flooredDifference = Beats - lastBeat;
            ceiledDifference = lastBeat + 1 - Beats;
            
            var errorInBeats = errorMargin / SecondsPerBeats;

            if (flooredDifference <= errorInBeats) return 1;
            else if (ceiledDifference <= errorInBeats) return 2;

            return 0;
        }
    }
}