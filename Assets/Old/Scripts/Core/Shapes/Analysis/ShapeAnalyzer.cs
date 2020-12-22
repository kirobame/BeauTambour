using System;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class ShapeAnalyzer
    {
        public event Action<Shape> OnEvaluationStart;

        public ShapeAnalyzer(Shape[] shapes)
        {
            this.shapes = shapes;
            
            for (var i = 0; i < shapes.Length; i++)
            {
                shapes[i].GenerateRuntimePoints();
                untrackedShapes.Add(i);
            }
        }
        
        private Shape[] shapes;
        
        private List<int> untrackedShapes = new List<int>();
        private List<ShapeAnalysis> trackedShapes = new List<ShapeAnalysis>();

        private Vector2 lastInput;

        public void Begin(Vector2 input) => lastInput = input;
        public ShapeAnalysis[] Evaluate(Vector2 input)
        {
            var settings = Repository.GetSingle<BeauTambourSettings>(Reference.Settings);
            
            for (var i = 0; i < untrackedShapes.Count; i++)
            {
                if (!shapes[untrackedShapes[i]].CanStartEvaluation(input)) continue;
                
                OnEvaluationStart?.Invoke(shapes[untrackedShapes[i]]);

                trackedShapes.Add(new ShapeAnalysis(shapes[untrackedShapes[i]], lastInput));
                untrackedShapes.RemoveAt(i);
                i--;
            }

            for (var i = 0; i < trackedShapes.Count; i++)
            {
                var analysis = trackedShapes[i];
                if (analysis.GoToNext && analysis.advancement == analysis.Source.RuntimePoints.Count - 1)
                {
                    analysis.advancement++;
                    
                    untrackedShapes.Add(shapes.IndexOf(analysis.Source));
                    trackedShapes.RemoveAt(i);
                    i--;
                    
                    continue;
                }
                else if (analysis.Error >= settings.RecognitionErrorThreshold)
                {
                    untrackedShapes.Add(shapes.IndexOf(analysis.Source));
                    trackedShapes.RemoveAt(i);
                    i--;

                    continue;
                }
                
                trackedShapes[i] = analysis.Source.Evaluate(analysis, input);
            }
            
            lastInput = input;
            return trackedShapes.ToArray();
        }
        public void Stop()
        {
            untrackedShapes.Clear();
            for (var i = 0; i < shapes.Length; i++) untrackedShapes.Add(i);
        
            trackedShapes.Clear();
        }
    }
}