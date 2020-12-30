using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeauTambour;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace Deprecated
{
    public class RythmHandler : MonoBehaviour
    {
        #region Encapsulated Types

        [EnumAddress]
        public enum EventType
        {
            OnBootUp,
            OnResume,
            OnBeat,
            OnTick,
            OnPause,
            OnShutdown,
        }
        #endregion

        public double BeatsPerSeconds => beatsPerMinutes / 60f;
        /// <summary>How much seconds fit in a beat.</summary> 
        public double SecondsPerBeats => 60f / beatsPerMinutes;
        /// <summary>How many beats fit in a minute.</summary> 
        public int BeatsPerMinutes => beatsPerMinutes;
        
        /// <summary>Is the rythm currently ongoing.</summary> 
        public bool IsActive { get; private set; }
        /// <summary>How much has time has passed since the beginning of the temp in seconds.</summary> 
        public double Time { get; private set; }
        /// <summary>How much has time has passed since the beginning of the temp in beats.</summary> 
        public double Beats { get; private set; }
        
        //--------------------------------------------------------------------------------------------------------------

        private double standardRythmMarginTolerance => Repository.GetSingle<BeauTambourSettings>(Reference.Settings).RythmMarginTolerance;
        
        [SerializeField] private int beatsPerMinutes;
        
        private double startTime;
        private double pauseTime;
        private int lastBeat = -1;

        private Queue<IRythmQueueable> earlyCalls = new Queue<IRythmQueueable>();
        private List<IRythmQueueable> processedCalls = new List<IRythmQueueable>();
        
        //--------------------------------------------------------------------------------------------------------------

        void Awake()
        {
            Event.Open(EventType.OnBootUp);
            Event.Open(EventType.OnResume);

            Event.Open<int>(EventType.OnBeat);
            Event.Open<double>(EventType.OnTick);
            
            Event.Open(EventType.OnPause);
            Event.Open(EventType.OnShutdown);

            Event.Register(Bootstrapper.EventType.OnBootup, BootUp);
        }
        
        void Update()
        {
            if (!IsActive) return;

            Time = AudioSettings.dspTime - startTime;
            Beats = Time / SecondsPerBeats;

            Event.Call<double>(EventType.OnTick, Time);
            ProcessCalls(call => call.Tick(Beats - call.Start - MathBt.Clamp(call.Offset, 0d, double.PositiveInfinity)));
            
            var roundedBeats = (int)Math.Floor(Beats);
            if (roundedBeats != lastBeat)
            {
                lastBeat = roundedBeats;
                Event.Call<int>(EventType.OnBeat, lastBeat);
                
                while (earlyCalls.Count > 0)
                {
                    var call = earlyCalls.Dequeue();
                    processedCalls.Add(call);
                }
                
                ProcessCalls(call => call.Beat(lastBeat - call.Start));
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        /// <summary>Starts the tempo.</summary> 
        public void BootUp()
        {
            Event.Call(EventType.OnBootUp);
            
            startTime = AudioSettings.dspTime;
            pauseTime = startTime;

            IsActive = true;
        }
        /// <summary>Ends the tempo.</summary> 
        public void ShutDown()
        {
            IsActive = false;

            Time = 0d;
            Beats = 0d;
            lastBeat = -1;
            
            Event.Call(EventType.OnShutdown);
        }
        
        /// <summary>Pauses the tempo and all its rythm calls.</summary>
        public void Pause()
        {
            IsActive = false;
            pauseTime = AudioSettings.dspTime;
            
            Event.Call(EventType.OnPause);
        }
        /// <summary>Resumes the tempo and all its rythm calls.</summary>
        public void Resume()
        {
            startTime += AudioSettings.dspTime - pauseTime;
            IsActive = true;
            
            Event.Call(EventType.OnResume);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        /// <summary>Computes the difference towards the last beat and the difference towards the next one.</summary> 
        public (double floored, double ceiled) GetDifferences()
        {
            var flooredDifference = Beats - lastBeat;
            var ceiledDifference = lastBeat + 1 - Beats;

            return (flooredDifference, ceiledDifference);
        }
        
        /// <summary>Checks if the current time's margins to the previous or next beat are within the given tolerance</summary> 
        public bool IsOnTempo(double toleranceMargin)
        {
            var differences = GetDifferences();
            var toleranceMarginInBeats = toleranceMargin / BeatsPerSeconds;

            if (differences.floored <= toleranceMarginInBeats || differences.ceiled <= toleranceMarginInBeats) return true;
            else return false;
        }
        public bool IsOnTempo() => IsOnTempo(standardRythmMarginTolerance);

        public bool IsOnLowerTempo() => IsOnLowerTempo(standardRythmMarginTolerance);
        public bool IsOnLowerTempo(double toleranceMargin)
        {
            var toleranceMarginInBeats = toleranceMargin / BeatsPerSeconds;
            var flooredDifference = Beats - lastBeat;

            return flooredDifference <= toleranceMarginInBeats;
        }

        public bool IsOnUpperTempo() => IsOnUpperTempo(standardRythmMarginTolerance);
        public bool IsOnUpperTempo(double toleranceMargin)
        {
            var toleranceMarginInBeats = toleranceMargin / BeatsPerSeconds;
            var ceiledDifference = lastBeat + 1 - Beats;

            return ceiledDifference <= toleranceMarginInBeats;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        public bool TryEnqueue(IRythmQueueable queueable, double toleranceMargin)
        {
            if (!IsActive || toleranceMargin <= 0 || toleranceMargin > SecondsPerBeats) return false;
            
            var differences = GetDifferences();
            var toleranceMarginInBeats = toleranceMargin / BeatsPerSeconds;

            if (differences.floored <= toleranceMarginInBeats)
            {
                queueable.Prepare(lastBeat, differences.floored);
                queueable.Beat(lastBeat - queueable.Start);
                
                processedCalls.Add(queueable);
                
                return true;
            } 
            else if (differences.ceiled <= toleranceMarginInBeats)
            {
                queueable.Prepare(lastBeat + 1, -differences.ceiled);
                earlyCalls.Enqueue(queueable);
                
                return true;
            }

            return false;
        }
        public void Enqueue(IRythmQueueable queueable)
        {
            var differences = GetDifferences();
            if (differences.floored <= differences.ceiled)
            {
                queueable.Prepare(lastBeat, differences.floored);
                queueable.Beat(lastBeat - queueable.Start);
                
                processedCalls.Add(queueable);
            }
            else
            {
                queueable.Prepare(lastBeat, -differences.ceiled);
                earlyCalls.Enqueue(queueable);
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------

        private void ProcessCalls(Action<IRythmQueueable> action)
        {
            var index = 0;
            while (index < processedCalls.Count)
            {
                var removalIndex = -1;
                
                for (var i = index; i < processedCalls.Count; i++)
                {
                    action(processedCalls[i]);
                    if (processedCalls[i].IsDone)
                    {
                        removalIndex = i;
                        break;
                    }
                    
                    index++;
                }
                
                if (removalIndex != -1) processedCalls.RemoveAt(removalIndex);
            }
        }
    }
}