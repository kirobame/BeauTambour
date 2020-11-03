using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewDrawOperation", menuName = "Beau Tambour/Operations/Draw")]
    public class DrawOperation : SingleOperation
    {
        [SerializeField] private int subDivision;
        [SerializeField] private float smoothing;
        
        private Dictionary<Shape, PoolableLineRenderer> drawings = new Dictionary<Shape, PoolableLineRenderer>();
        
        public override void OnStart(EventArgs inArgs)
        {
            if (!(inArgs is ShapeEventArgs shapeEventArgs) || drawings.ContainsKey(shapeEventArgs.Value)) return;
            
            var pool = Repository.GetSingle<LineRendererPool>(Reference.LinePool);
            var drawing = pool.RequestSinglePoolable();
            
            var points = shapeEventArgs.Value.Define(subDivision);
            drawing.SetPoints(points);
            drawing.StartFill();
            
            drawings.Add(shapeEventArgs.Value, drawing);
        }
        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is ShapeAnalysisResultsEventArgs shapeAnalysisResultsEventArgs)) return;
            
            var unused = new List<Shape>(drawings.Keys);
            var removals = new List<Shape>();

            foreach (var shapeAnalysisResult in shapeAnalysisResultsEventArgs.Value)
            {
                unused.Remove(shapeAnalysisResult.Shape);
               
                if (shapeAnalysisResult.IsIncorrect) removals.Add(shapeAnalysisResult.Shape);
                else drawings[shapeAnalysisResult.Shape].TryUpdateFill(smoothing, shapeAnalysisResult.Analysis.GlobalRatio);
            }

            foreach (var shape in removals)
            {
                Debug.Log("Incorrect");
                
                drawings[shape].EndFill();
                drawings.Remove(shape);
            }
            foreach (var shape in unused) drawings[shape].TryUpdateFill(smoothing);
        }
        public override void OnEnd(EventArgs inArgs)
        {
            foreach (var drawing in drawings.Values)
            {
                Debug.Log("End");
                drawing.EndFill();
            };
            drawings.Clear();
        }
    }
}