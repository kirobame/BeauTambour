using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewDrawOperation", menuName = "Beau Tambour/Operations/Draw")]
    public class DrawOperation : SingleOperation
    {
        [SerializeField] private int subDivision;

        private bool isDrawing;
        private (Shape shape, PoolableDrawing drawing) currentPair;

        public override void OnStart(EventArgs inArgs)
        {
            if (isDrawing || !(inArgs is ShapeEventArgs shapeEventArgs)) return;
            
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
                    return;
                }
                else if (!analysis.IsValid)
                {
                    End(false);
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
        
        /*[SerializeField] private int subDivision;
        [SerializeField] private float smoothing;
        
        private Dictionary<Shape, PoolableDrawing> drawings = new Dictionary<Shape, PoolableDrawing>();
        
        public override void OnStart(EventArgs inArgs)
        {
            if (drawings.Values.Any(item => item.Current > 0.025f)) return;
            
            if (!(inArgs is ShapeEventArgs shapeEventArgs) || drawings.ContainsKey(shapeEventArgs.Value)) return;
            
            var pool = Repository.GetSingle<DrawingPool>(Pool.Drawing);
            var drawing = pool.RequestSinglePoolable();

            var drawingsParent = Repository.GetSingle<Transform>(Parent.Drawings);
            drawing.transform.SetParent(drawingsParent);

            drawing.transform.localPosition = Vector2.zero;
            
            drawing.AssignShape(shapeEventArgs.Value, subDivision);
            drawings.Add(shapeEventArgs.Value, drawing);
        }
        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is ShapeAnalyzerResultEventArgs shapeAnalyzerResultEventArgs)) return;
            
            var unused = new List<Shape>(drawings.Keys);
            var removals = new List<Shape>();
            
            ShapeAnalysis match = null; 
            
            var values = shapeAnalyzerResultEventArgs.Value.Where(analysis => drawings.ContainsKey(analysis.Source));
            foreach (var shapeAnalysis in values)
            {
                if (shapeAnalysis.IsComplete) match = shapeAnalysis;
                
                unused.Remove(shapeAnalysis.Source);
               
                if (!shapeAnalysis.IsValid && !shapeAnalysis.IsComplete) removals.Add(shapeAnalysis.Source);
                else drawings[shapeAnalysis.Source].Draw(smoothing, shapeAnalysis.GlobalRatio);
            }
            
            /*foreach (var shape in removals)
            {
                drawings[shape].Complete(false);
                drawings.Remove(shape);
            }
            if (removals.Count > 0)
            {
                var shape = removals.First();
                
                drawings[shape].Complete(false);
                drawings.Remove(shape);
                
                bindedHandler.OnCanceled();
            }

            foreach (var shape in unused) drawings[shape].Draw(smoothing);

            if (match != null)
            {
                var phaseHandler = Repository.GetSingle<PhaseHandler>(Reference.PhaseHandler);
                var outcomePhase = phaseHandler.Get<OutcomePhase>(PhaseType.Outcome);
                
                outcomePhase.BeginNote();
                outcomePhase.EnqueueNoteAttribute(new EmotionAttribute(match.Source.Emotion));

                drawings[match.Source].Complete(true);
                drawings.Remove(match.Source);
                
                bindedHandler.OnCanceled();
            }
        }
        public override void OnEnd(EventArgs inArgs)
        {
            foreach (var drawing in drawings.Values) drawing.Complete(false);
            drawings.Clear();
        }*/
    }
}