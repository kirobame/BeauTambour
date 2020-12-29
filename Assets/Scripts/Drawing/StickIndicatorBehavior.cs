using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flux;
using Event = Flux.Event;
using Deprecated;
using System;

namespace BeauTambour
{
    public class StickIndicatorBehavior : MonoBehaviour
    {  
        [EnumAddress]
        public enum EventType
        {
            OnMagnetize,
            OnUnMagnetize,
            OnDrawing,
            OnDelayedDrawing,
            OnDrawingProgress,
            OnShapeMatch,
            OnShapeLoss,
            OnValidation
        }

        public enum DrawingState
        {
            Selection,
            Drawing,
            Validation
        }

        [SerializeField] private List<Section> sections;
        [SerializeField] private float smoothing = 0.1f;

        private DrawingState state;

        private Vector2 velocity;
        private Transform stickIndicator;
        private Vector2 cursorPos;
        private Section magnetizedSection;

        private ShapeAnalyzer analyzer;
        private Coroutine delayedActivationRoutine;
        [SerializeField] private float activationDelay = 0.2f;
        [SerializeField] private int subDivision;
        private PoolableDrawing drawing;
        [SerializeField] private PoolableDrawing prefab;



        //----------------------UNITY LIFE STEP---------------------------------------
        private void Awake()
        {
            magnetizedSection = null;
            state = DrawingState.Selection;
            Event.Open(EventType.OnMagnetize);
            Event.Open(EventType.OnUnMagnetize);
            Event.Open(EventType.OnDelayedDrawing);
            Event.Open(EventType.OnDrawing);
            Event.Open<float>(EventType.OnDrawingProgress);

            Event.Register<Vector2>(StickIndicatorOperation.EventType.OnUpdate, OnUpdate);
            Event.Register<Vector2>(StickIndicatorOperation.EventType.OnEnd, OnEnd);

            Event.Register(SelectionValidationOperation.EventType.OnStart, OnValidation);
        }

        private void Start()
        {
            stickIndicator = Repository.GetSingle<Transform>(Reference.StickIndicator);
        }

        //----------------------EVENT STEP---------------------------------------
        private void OnUpdate(Vector2 input)
        {
            switch (state)
            {
                case DrawingState.Selection:
                    HandleSelection(input);
                    break;
                case DrawingState.Drawing:
                    HandleDrawing(input);
                    break;
                case DrawingState.Validation:
                    break;
                default:
                    break;
            }
        }

        private void OnEnd(Vector2 input)
        {
            switch (state)
            {
                case DrawingState.Selection:
                    HandleSelectionInputEnd(input);
                    break;
                case DrawingState.Drawing:
                    Place(input, false);
                    analyzer.Stop();
                    break;
                case DrawingState.Validation:
                    break;
                default:
                    break;
            }
            
        }

        private void OnValidation()
        {
            state = DrawingState.Drawing;
        }

        //----------------------FUNCTIONS---------------------------------------
        private void HandleSelection(Vector2 input)
        {
            if (magnetizedSection != null)
            {
                MagnetizeCursorToSection();
                if (!magnetizedSection.IsInCatchZone(input))
                {
                    magnetizedSection = null;
                    Event.Call(EventType.OnUnMagnetize);
                    return;
                }
            }
            else
            {
                Place(input, true);
            }
            foreach (Section section in sections)
            {
                if (section.IsInSection(cursorPos))
                {
                    section.ScaleAnchor(cursorPos);
                    if (magnetizedSection == null && section.IsInCatchZone(input))
                    {
                        magnetizedSection = section;
                        Event.Call(EventType.OnMagnetize);
                    }
                }
                else
                {
                    section.SmoothScaleAnchorReset();
                }
            }
        }

        private void HandleSelectionInputEnd(Vector2 input)
        {
            if (magnetizedSection != null)
            {
                magnetizedSection = null;
                Event.Call(EventType.OnUnMagnetize);
            }
            Place(input, false);
            foreach (Section section in sections)
            {
                section.ScaleAnchor(cursorPos);
            }
        }

        private void HandleDrawing(Vector2 input)
        {
            if (analyzer == null)
            {
                analyzer = new ShapeAnalyzer(magnetizedSection.Shape);
                analyzer.OnEvaluationStart += shape => BeginDrawing(shape);
                analyzer.Begin(input);
            }
            Place(input, true);
            ShapeAnalysis analysis = analyzer.Evaluate(input);

            Event.Call<float>(EventType.OnDrawingProgress,analysis.GlobalRatio);
            if (delayedActivationRoutine != null) return;

            drawing.Draw(0f, analysis.GlobalRatio);
            /*if (analysis.IsComplete)
            {
                EndDrawing(true);

                var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
                var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);

                if (!outcomePhase.IsNoteBeingProcessed) return;

                outcomePhase.EnqueueNoteAttribute(new EmotionAttribute(analysis.Source.Emotion));
                outcomePhase.CompleteNote();

                return;
            }
            else if (!analysis.IsValid)
            {
                EndDrawing(false);
                return;
            }     */       
        }

        private void Place(Vector2 input, bool smooth)
        {
            if (smooth)
            {
                stickIndicator.localPosition = Vector2.SmoothDamp(stickIndicator.localPosition, input, ref velocity, smoothing);
            }
            else
            {
                stickIndicator.localPosition = input;
            }
            cursorPos = stickIndicator.localPosition;
        }

        private void MagnetizeCursorToSection()
        {
            Place(magnetizedSection.Anchor.localPosition, true);
            //magnetizedSection.ScaleAnchor(magnetizedSection.Anchor.localPosition);
        }

        private void BeginDrawing(Shape shape)
        {
            if (delayedActivationRoutine != null) return;
            Event.Call(EventType.OnDrawing);
            //CallShapeEvent(shapeEventArgs.Value.Emotion);
            Debug.Log("BEGIN DRAWING");
            delayedActivationRoutine = StartCoroutine(DelayedActivationRoutine(shape));
        }

        public void EndDrawing(bool outcome)
        {
            if (outcome) Event.Call(EventType.OnShapeMatch);
            else Event.Call(EventType.OnShapeLoss);

            drawing.Complete(outcome);
            drawing = null;

            analyzer = null;
        }

        private IEnumerator DelayedActivationRoutine(Shape shape)
        {
            yield return new WaitForSeconds(activationDelay);

            var pool = Repository.GetSingle<DrawingPool>(Deprecated.Pool.Drawing);
            drawing = pool.RequestSinglePoolable(prefab);

            var drawingsParent = Repository.GetSingle<Transform>(Parent.Drawings);
            drawing.transform.SetParent(drawingsParent);
            drawing.transform.localPosition = Vector2.zero;

            drawing.AssignShape(shape, subDivision);

            Event.Call(EventType.OnDelayedDrawing);
            delayedActivationRoutine = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var item in sections)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(item.Anchor.position, item.CatchZonedistance);
            }
        }
#endif
    }
}