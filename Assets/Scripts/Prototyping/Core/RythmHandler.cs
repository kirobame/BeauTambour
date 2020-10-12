using System;
using System.Collections;
using System.Collections.Generic;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class RythmHandler : MonoBehaviour, IBootable
    {
        #region Encapsuled Types

        private abstract class TimedAction
        {
            public abstract bool IsDone { get; }
            
            protected double time;
            protected double offset;

            public virtual void SetDuration(double offset, double duration) => this.offset = offset;
            public virtual void TryInvoke(double delta) => time += delta;
        }

        private class PlainAction : TimedAction
        {
            public PlainAction(Action<int,double> action) => this.action = action;

            public override bool IsDone => lastBeat == beatGoal;

            private Action<int,double> action;
            
            private int lastBeat = -1;
            private int beatGoal;

            public override void SetDuration(double offset, double duration)
            {
                if (offset > 0)
                {
                    beatGoal = (int)Math.Ceiling(duration - offset);
                    
                    TryInvoke(0d);
                    this.offset = offset;
                }
                else beatGoal = (int)Math.Floor(duration - offset);
            }
            public override void TryInvoke(double delta)
            {
                var beat = (int)Math.Floor(time + offset);
                if (lastBeat != beat)
                {
                    action(beat, offset);
                    lastBeat = beat;
                }
                
                base.TryInvoke(delta);
            }
        }
        private class StandardAction : TimedAction
        {
            public StandardAction(Action<double,double> action) => this.action = action;

            public override bool IsDone => time >= goal;

            private Action<double,double> action;

            private double goal;

            public override void SetDuration(double offset, double duration)
            {
                if (offset > 0) this.offset = offset;
                goal = duration;
            }
            public override void TryInvoke(double delta)
            {
                action(time, offset);
                base.TryInvoke(delta);
            }
        }
        #endregion
        
        /// <summary>The allowed delay for an input to be considered on time with the rythm.</summary> 
        public const double StandardErrorMargin = 0.175d;
        
        //--------------------------------------------------------------------------------------------------------------
        
        /// <summary>Callback for whenever the rythm hits.</summary> 
        public event Action<double> OnBeat;

        /// <summary>Called each frame if active & has for parameter the time that has passed since last frame
        /// in the AudioSystem.</summary> 
        public event Action<double> OnTimeAdvance;
        
        //--------------------------------------------------------------------------------------------------------------
        
        /// <summary>Is the rythm currently ongoing.</summary> 
        public bool IsActive => audioSource.isPlaying;
        
        /// <summary>How much seconds fit in a beat.</summary> 
        public double SecondsPerBeats => 60f / beatsPerMinutes;
        /// <summary>How many beats fit in a minute.</summary> 
        public int BeatsPerMinutes => beatsPerMinutes;
        
        /// <summary>How much has time has passed since the beginning of the temp in seconds.</summary> 
        public double Time { get; private set; }
        /// <summary>How much has time has passed since the beginning of the temp in beats.</summary> 
        public double Beats { get; private set; }
        
        //--------------------------------------------------------------------------------------------------------------

        int IBootable.Priority => bootUpPriority;
        
        //--------------------------------------------------------------------------------------------------------------
        
        [SerializeField] private int bootUpPriority;
        
        [Space, SerializeField] private AudioSource audioSource;
        [SerializeField] private int beatsPerMinutes;

        private double startTime;
        private double pauseTime;
        private int lastBeat = -1;
        
        private Queue<TimedAction> earlyActions = new Queue<TimedAction>();
        private List<TimedAction> timedActions = new List<TimedAction>();
        
        //--------------------------------------------------------------------------------------------------------------

        void Update()
        {
            if (!IsActive) return;

            Time = AudioSettings.dspTime - startTime;
            
            var beats = Time / SecondsPerBeats;
            var delta = beats - Beats;
            Beats = beats;

            OnTimeAdvance?.Invoke(delta);
            HandleTimedActions(delta);
            
            var roundedBeats = (int)Math.Floor(Beats);
            if (roundedBeats != lastBeat)
            {
                lastBeat = roundedBeats;
                OnBeat?.Invoke(lastBeat);

                while (earlyActions.Count > 0)
                {
                    var timedAction = earlyActions.Dequeue();
                    timedActions.Add(timedAction);
                }
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        /// <summary>Starts the tempo.</summary> 
        [FoldoutGroup("Methods"), Button("Play")]
        public void BootUp()
        {
            startTime = AudioSettings.dspTime;
            audioSource.Play();
        }
        /// <summary>Ends the tempo.</summary> 
        [FoldoutGroup("Methods"), Button("Stop")]
        public void ShutDown()
        {
            audioSource.Stop();

            Time = 0d;
            Beats = 0d;
            
            lastBeat = -1;
        }

        /// <summary>Pauses the tempo and all its rythm calls.</summary>
        [FoldoutGroup("Methods"), Button]
        public void Pause()
        {
            pauseTime = AudioSettings.dspTime;
            audioSource.Pause();
        }
        /// <summary>Resumes the tempo and all its rythm calls.</summary>
        [FoldoutGroup("Methods"), Button]
        public void Resume()
        {
            startTime += AudioSettings.dspTime - pauseTime;
            audioSource.UnPause();
        }
        
        //--------------------------------------------------------------------------------------------------------------

        /// <summary>Computes the difference towards the last beat and the difference towards the next one.</summary> 
        public (double floored, double ceiled) GetDifferences()
        {
            var flooredDifference = Beats - lastBeat;
            var ceiledDifference = lastBeat + 1 - Beats;

            return (flooredDifference, ceiledDifference);
        }

        public bool IsOnTempo(double errorMargin = StandardErrorMargin)
        {
            var differences = GetDifferences();
            var errorInBeats = errorMargin / SecondsPerBeats;

            if (differences.floored <= errorInBeats || differences.ceiled <= errorInBeats) return true;
            else return false;
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        /// <summary>If the timing is correct, queues a callback which will be called on tempo (duration + 1) counting
        /// the first beat on which the call was made.</summary>
        /// <param name="action">The callback.</param>
        /// <param name="duration">The number of times the callback will be called.</param>
        /// <param name="errorMargin">The maximum difference in seconds compared to the next or previous beat for the
        /// call to be considered correct.</param>
        public bool TryStandardEnqueue(Action<double,double> action, int duration, double errorMargin = StandardErrorMargin)
        {
            var standardAction = new StandardAction(action);
            return TryEnqueue(standardAction, duration, errorMargin);
        }
        /// <summary>If the timing is correct, queues a callback which will be called each frame for a certain duration
        /// in beats.</summary>
        /// <param name="action">The callback.</param>
        /// <param name="duration">The duration in beats during which the callback will be called.</param>
        /// <param name="errorMargin">The maximum difference in seconds compared to the next or previous beat for the
        /// call to be considered correct.</param>
        public bool TryPlainEnqueue(Action<int,double> action, int duration, double errorMargin = StandardErrorMargin)
        {
            var plainAction = new PlainAction(action);
            return TryEnqueue(plainAction, duration, errorMargin);
        }
        private bool TryEnqueue(TimedAction timedAction, int duration, double errorMargin)
        {
            if (!IsActive || errorMargin <= 0 || errorMargin > SecondsPerBeats) return false;
            
            var differences = GetDifferences();
            var errorInBeats = errorMargin / SecondsPerBeats;
            
            if (differences.floored <= errorInBeats) return EnqueueLateCall(timedAction, duration, differences.floored);
            else if (differences.ceiled <= errorInBeats) return EnqueueEarlyCall(timedAction, duration, differences.ceiled);
            
            return false;
        }

        /// <summary>Queues a callback which will be called on tempo (duration + 1) counting the first beat on which
        /// the call was made.</summary>
        /// <param name="action">The callback.</param>
        /// <param name="duration">The number of times the callback will be called.</param>
        public void MakePlainEnqueue(Action<int,double> action, int duration) => Enqueue(new PlainAction(action), duration);
        /// <summary>Queues a callback which will be called each frame for a certain duration in beats.</summary>
        /// <param name="action">The callback.</param>
        /// <param name="duration">The duration in beats during which the callback will be called.</param>
        public void MakeStandardEnqueue(Action<double,double> action, int duration) => Enqueue(new StandardAction(action), duration);
        private void Enqueue(TimedAction timedAction, int duration)
        {
            var differences = GetDifferences();
            if (differences.floored <= differences.ceiled) EnqueueLateCall(timedAction, duration, differences.floored);
            else EnqueueEarlyCall(timedAction, duration, differences.ceiled);
        }
        
        private bool EnqueueEarlyCall(TimedAction timedAction, int duration, double difference)
        {
            timedAction.SetDuration(-difference, duration);
            earlyActions.Enqueue(timedAction);
                
            return true;
        }
        private bool EnqueueLateCall(TimedAction timedAction, int duration, double difference)
        {
            timedAction.SetDuration(difference, duration - difference);
            timedActions.Add(timedAction);
                
            return true;
        }
        
        private void HandleTimedActions(double delta)
        {
            var index = 0;
            while (index < timedActions.Count)
            {
                var removalIndex = -1;
                
                for (var i = index; i < timedActions.Count; i++)
                {
                    timedActions[i].TryInvoke(delta);
                    if (timedActions[i].IsDone)
                    {
                        removalIndex = i;
                        break;
                    }
                    
                    index++;
                }
                
                if (removalIndex != -1) timedActions.RemoveAt(removalIndex);
            }
        }
    }
}