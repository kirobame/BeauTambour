using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeauTambour;
using Flux;
using Shapes;
using UnityEngine;
using Event = Flux.Event;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewDrawOperation", menuName = "Beau Tambour/Operations/Draw")]
    public class DrawOperation : SingleOperation
    {
        #region Encapsulated Types

        [EnumAddress]
        public enum EventType
        {
            OnStart,
            
            OnShapeMatch,
            OnShapeLoss,
            OnShapeEnd,
            
            OnDelayedStart,
            
            OnJoy,
            OnTrust,
            OnFear,
            OnSurprise,
            OnSadness,
            OnDisgust,
            OnAnger,
            OnAnticipation
        }
        #endregion

        [SerializeField] private int subDivision;
        [SerializeField] private float activationDelay = 0.2f;
        [SerializeField] private PoolableDrawing drawingPrefab;

        private bool isDrawing;
        private bool isBusy;

        private Coroutine delayedActivationRoutine;
        private (Shape shape, PoolableDrawing drawing) currentPair;

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);

            Event.Open(EventType.OnStart);
            Event.Open(EventType.OnShapeMatch);
            Event.Open(EventType.OnShapeLoss);
            Event.Open(EventType.OnDelayedStart);
            Event.Open(EventType.OnShapeEnd);

            Event.Open(EventType.OnJoy);
            Event.Open(EventType.OnTrust);
            Event.Open(EventType.OnFear);
            Event.Open(EventType.OnSurprise);
            Event.Open(EventType.OnSadness);
            Event.Open(EventType.OnDisgust);
            Event.Open(EventType.OnAnger);
            Event.Open(EventType.OnAnticipation);
            
            Event.Register(EventType.OnShapeMatch,()=> { Event.Call(EventType.OnShapeEnd); });
            Event.Register(EventType.OnShapeLoss,()=> { Event.Call(EventType.OnShapeEnd); });

            Repository.Reference(this, Reference.DrawOperation);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Repository.Dereference(this, Reference.DrawOperation);
        }

        public override void OnStart(EventArgs inArgs)
        {
            if (isBusy || isDrawing || delayedActivationRoutine != null || !(inArgs is ShapeEventArgs shapeEventArgs)) return;
            
            isBusy = true;
            
            Event.Call(EventType.OnStart);
            //CallShapeEvent(shapeEventArgs.Value.Emotion);

            delayedActivationRoutine = hook.StartCoroutine(DelayedActivationRoutine(shapeEventArgs));
        }
        private IEnumerator DelayedActivationRoutine(ShapeEventArgs shapeEventArgs)
        {
            yield return new WaitForSeconds(activationDelay);
            
            var pool = Repository.GetSingle<DrawingPool>(Pool.Drawing);
            var drawing = pool.RequestSinglePoolable(drawingPrefab);

            var drawingsParent = Repository.GetSingle<Transform>(Parent.Drawings);
            drawing.transform.SetParent(drawingsParent);
            drawing.transform.localPosition = Vector2.zero;

            drawing.AssignShape(shapeEventArgs.Value, subDivision);

            currentPair = (shapeEventArgs.Value, drawing);
            isDrawing = true;

            Event.Call(EventType.OnDelayedStart);
            delayedActivationRoutine = null;
        }
        

        public override void OnUpdate(EventArgs inArgs)
        {
            if (delayedActivationRoutine != null) return;
            if (!(inArgs is ShapeAnalyzerResultEventArgs shapeAnalyzerResultEventArgs)) return;
            
            var values = shapeAnalyzerResultEventArgs.Value;
            foreach (var analysis in values)
            {
                if (analysis.Source != currentPair.shape) continue;
                
                currentPair.drawing.Draw(0f, analysis.GlobalRatio);
                if (analysis.IsComplete)
                {
                    End(true);

                    var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
                    var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);
                    
                    if (!outcomePhase.IsNoteBeingProcessed) return;
                    
                    //outcomePhase.EnqueueNoteAttribute(new EmotionAttribute(analysis.Source.Emotion));
                    outcomePhase.CompleteNote();
                    
                    return;
                }
                else if (!analysis.IsValid)
                {
                    End(false);
                    return;
                }
            }
        }

        public void End(bool outcome)
        {
            if (!isDrawing) return;
            
            if (outcome) Event.Call(EventType.OnShapeMatch);
            else Event.Call(EventType.OnShapeLoss);
            
            //currentPair.drawing.Complete(outcome);
            currentPair = (null,null);
            
            isDrawing = false;
            if (bindable is InputHandler handler) handler.OnCanceled();
        }

        public override void OnEnd(EventArgs inArgs)
        {
            isBusy = false;
            if (!isDrawing) return;
            
            Event.Call(EventType.OnShapeLoss);
            //currentPair.drawing.Complete(false);
            currentPair = (null,null);
            
            isDrawing = false;
        }

        private static void CallShapeEvent(Emotion emotion)
        {
            switch (emotion)
            {
                case Emotion.Joy:
                    Event.Call(EventType.OnJoy);
                    break;
                case Emotion.Anger:
                    Event.Call(EventType.OnAnger);
                    break;
                case Emotion.Sadness:
                    Event.Call(EventType.OnSadness);
                    break;
                case Emotion.Fear:
                    Event.Call(EventType.OnFear);
                    break;
                case Emotion.Disgust:
                    Event.Call(EventType.OnDisgust);
                    break;
                case Emotion.Surprise:
                    Event.Call(EventType.OnSurprise);
                    break;
                case Emotion.Anticipation:
                    Event.Call(EventType.OnAnticipation);
                    break;
                case Emotion.Trust:
                    Event.Call(EventType.OnTrust);
                    break;
                default:
                    break;
            }
        }
    }
}