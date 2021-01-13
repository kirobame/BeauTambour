using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using Event = Flux.Event;
using Deprecated;
using System;

namespace BeauTambour
{
    public class DrawingHandler : MonoBehaviour
    {
        #region Encapuslated Types

        private enum State
        {
            Picking,
            Magnetized,
            Transitioning,
            Drawing,
            Ended,
        }
        #endregion

        public float Radius => radius;
        
        [SerializeField] private float radius;
        [SerializeField] private int definition;
        
        [SerializeField] private EmotionAnchor[] anchors;
        [SerializeField] private Transform indicator;
        [SerializeField] private PoolableAnimation validationPrefab;

        [Space, SerializeField] private Vector2 magnetizationAngles;
        [SerializeField] private Vector2 magnetizationLengths;
        
        private State state;
        private Vector2 cachedInput;

        private int magnetizedAnchorIndex;
        private ShapeAnalyzer analyzer;
        private PoolableDrawing drawing;

        //private Vector2 dummy;

        void Awake()
        {
            Repository.Reference(this, References.DrawingHandler);
            
            Event.Open(GameEvents.OnEmotionLatched);
            Event.Open(GameEvents.OnEmotionUnlatched);
            Event.Open(GameEvents.OnEmotionPicked);
            Event.Open(GameEvents.OnEmotionDrawingStart);
            Event.Open(GameEvents.OnEmotionDrawingCancellation);
            Event.Open(GameEvents.OnEmotionCancellationDone);
            Event.Open(GameEvents.OnEmotionDrawingEnd);

            Event.Register(GameEvents.OnEmotionDrawingStart, OnEmotionDrawingStart);
            Event.Register(GameEvents.OnEmotionCancellationDone, OnEmotionCancellationDone);

            Event.Register($"{PhaseCategory.EmotionDrawing}.{PhaseCallback.Start}", () => indicator.position = transform.position);
            
            analyzer = new ShapeAnalyzer();
            state = State.Picking;
        }

        public void TryBeginDrawing()
        {
            if (state != State.Magnetized) return;

            state = State.Transitioning;
            Event.Call(GameEvents.OnEmotionPicked);
        }
        public void TryCancelDrawing()
        {
            if (state != State.Drawing) return;

            state = State.Transitioning;
            
            drawing.Stop();
            Event.Call(GameEvents.OnEmotionDrawingCancellation);
        }

        public void Execute(Vector2 input)
        {
            /*if (Vector2.Distance(input, dummy) >= 0.4f) Debug.Log($"MAJOR -||- FROM {dummy} --> TO {input}");
            dummy = input;*/
                
            cachedInput = input.normalized;
            
            input *= radius;
            indicator.localPosition = input;

            switch (state)
            {
                case State.Picking: HandlePicking(input);
                    break;
                
                case State.Magnetized: HandleMagnetization(input);
                    break;
                
                case State.Transitioning: HandleTransition(input);
                    break;
                
                case State.Drawing: HandleDrawing(input); 
                    break;
                
                case State.Ended: HandleEnding(input); 
                    break;
            }
        }

        private void HandlePicking(Vector2 input)
        {
            for (var i = 0; i < anchors.Length; i++)
            {
                if (!anchors[i].IsMagnetizing(transform.position, input, magnetizationAngles.x, magnetizationLengths.x)) continue;

                var animationPool = Repository.GetSingle<AnimationPool>(References.AnimationPool);
                var animation = animationPool.RequestSingle(validationPrefab);

                animation.ResetTrigger("In");
                animation.ResetTrigger("Out");

                animation.gameObject.name = Guid.NewGuid().ToString();

                animation.transform.position = anchors[i].transform.position;
                animation.SetTrigger("In");

                magnetizedAnchorIndex = i;
                state = State.Magnetized;
                
                Event.Call(GameEvents.OnEmotionLatched);
                break;
            }
        }
        private void HandleMagnetization(Vector2 input)
        {
            foreach (var anchor in anchors) anchor.IsMagnetizing(transform.position, input, magnetizationAngles.x, magnetizationLengths.x);
            if (!anchors[magnetizedAnchorIndex].HasLostMagnetization(transform.position, input, magnetizationAngles.y, magnetizationLengths.y)) return;
            
            state = State.Picking;
            Event.Call(GameEvents.OnEmotionUnlatched);
        }
        private void HandleTransition(Vector2 input) { }
        private void HandleDrawing(Vector2 input)
        {
            var analysis = analyzer.Evaluate(input / radius);
            drawing.Draw(0.075f, analysis.GlobalRatio);

            if (analysis.IsComplete)
            {
                Event.Call<string>(GameEvents.OnFrogFeedback, "Hit.0");
                drawing.Complete();

                state = State.Ended;
                Event.Call(GameEvents.OnEmotionDrawingEnd);
            }
        }
        private void HandleEnding(Vector2 input)
        {
            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            phaseHandler.Play(PhaseCategory.NoteValidation);
            
            state = State.Picking;

            GameState.Note.emotion = anchors[magnetizedAnchorIndex].Shape.Emotion;
            Event.Open(GameEvents.OnEmotionDrawingEnd);
        }

        void OnEmotionDrawingStart()
        {
            if (state != State.Transitioning) return;

            foreach (var anchor in anchors) anchor.Reboot();
            
            var pool = Repository.GetSingle<DrawingPool>(References.DrawingPool);
            drawing = pool.RequestSinglePoolable();

            var drawingsParent = Repository.GetSingle<Transform>(References.DrawingsParent);
            drawing.transform.SetParent(drawingsParent);
            drawing.transform.localScale = Vector3.one;
            drawing.transform.localPosition = Vector2.zero;

            drawing.AssignShape(anchors[magnetizedAnchorIndex].Shape, definition);
            
            analyzer.Begin(anchors[magnetizedAnchorIndex].Shape, cachedInput);
            state = State.Drawing;
        }
        void OnEmotionCancellationDone() => state = State.Picking;

        public bool TryGetDrawnShape(out Shape shape)
        {
            if (state == State.Drawing)
            {
                shape = anchors[magnetizedAnchorIndex].Shape;
                return true;
            }
            else
            {
                shape = null;
                return false;
            }
        }
    }
}