using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewDrawOperation", menuName = "Beau Tambour/Operations/Draw")]
    public class DrawOperation : SingleOperation
    {
        #region Encapsulated Types

        [EnumAddress]
        public enum EventType
        {
            OnStart,
            OnShapeMatch,
            OnShapeLoss
        }

        #endregion
        
        [SerializeField] private int subDivision;

        private bool isDrawing;
        private (Shape shape, PoolableDrawing drawing) currentPair;

        public override void Initialize(OperationHandler operationHandler)
        {
            base.Initialize(operationHandler);

            Event.Open(EventType.OnStart);
            Event.Open(EventType.OnShapeMatch);
            Event.Open(EventType.OnShapeLoss);
        }

        public override void OnStart(EventArgs inArgs)
        {
            if (isDrawing || !(inArgs is ShapeEventArgs shapeEventArgs)) return;
            
            Event.Call(EventType.OnStart);
            
            var pool = Repository.GetSingle<DrawingPool>(Pool.Drawing);
            var drawing = pool.RequestSinglePoolable();
            
            var drawingsParent = Repository.GetSingle<Transform>(Parent.Drawings);
            drawing.transform.SetParent(drawingsParent);
            drawing.transform.localPosition = Vector2.zero;
            
            drawing.AssignShape(shapeEventArgs.Value, subDivision);

            currentPair = (shapeEventArgs.Value, drawing);
            isDrawing = true;
        }

        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is ShapeAnalyzerResultEventArgs shapeAnalyzerResultEventArgs)) return;
            
            var values = shapeAnalyzerResultEventArgs.Value;
            foreach (var analysis in values)
            {
                if (analysis.Source != currentPair.shape) continue;
                
                currentPair.drawing.Draw(0f, analysis.GlobalRatio);
                if (analysis.IsComplete)
                {
                    var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
                    var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);
                
                    outcomePhase.BeginNote();
                    outcomePhase.EnqueueNoteAttribute(new EmotionAttribute(analysis.Source.Emotion));

                    End(true);
                    Event.Call(EventType.OnShapeMatch);
                    
                    return;
                }
                else if (!analysis.IsValid)
                {
                    End(false);
                    Event.Call(EventType.OnShapeLoss);
                    
                    return;
                }
            }
        }

        private void End(bool outcome)
        {
            currentPair.drawing.Complete(outcome);
            currentPair = (null,null);
            
            isDrawing = false;
            bindedHandler.OnCanceled();
        }
        public override void OnEnd(EventArgs inArgs)
        {
            if (!isDrawing) return;
            
            currentPair.drawing.Complete(false);
            currentPair = (null,null);
            
            isDrawing = false;
        }
    }
}