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
        [SerializeField] private float smoothing;
        
        private Dictionary<Shape, PoolableDrawing> drawings = new Dictionary<Shape, PoolableDrawing>();
        
        public override void OnStart(EventArgs inArgs)
        {
            if (drawings.Values.Any(item => item.Current > 0.1f)) return;
            
            if (!(inArgs is ShapeEventArgs shapeEventArgs) || drawings.ContainsKey(shapeEventArgs.Value)) return;
            
            var pool = Repository.GetSingle<DrawingPool>(Reference.DrawingPool);
            var drawing = pool.RequestSinglePoolable();
            
            drawing.AssignShape(shapeEventArgs.Value, subDivision);
            drawings.Add(shapeEventArgs.Value, drawing);
        }
        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is ShapeAnalyzerResultEventArgs shapeAnalyzerResultEventArgs)) return;
            
            var unused = new List<Shape>(drawings.Keys);
            var removals = new List<Shape>();
            
            var values = shapeAnalyzerResultEventArgs.Value.Where(analysis => drawings.ContainsKey(analysis.Source));
            foreach (var shapeAnalysis in values)
            {
                unused.Remove(shapeAnalysis.Source);
               
                if (!shapeAnalysis.IsValid) removals.Add(shapeAnalysis.Source);
                else drawings[shapeAnalysis.Source].Draw(smoothing, shapeAnalysis.GlobalRatio);
            }

            foreach (var shape in removals)
            {
                drawings[shape].Complete();
                drawings.Remove(shape);
            }
            foreach (var shape in unused) drawings[shape].Draw(smoothing);
        }
        public override void OnEnd(EventArgs inArgs)
        {
            foreach (var drawing in drawings.Values) drawing.Complete();
            drawings.Clear();
        }
    }
}