using System;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class ShapeAnalyzer
    {
        #region Encapsulated Types

        private struct TrackedShape
        {
            public TrackedShape(int index, Vector2 lastInput)
            {
                shapeIndex = index;
                analysis = new ShapeAnalysis() {position = lastInput};
            }
            
            public int shapeIndex;
            public ShapeAnalysis analysis;
        }
        #endregion

        public event Action<Shape> OnEvaluationStart;

        public ShapeAnalyzer(Shape[] shapes)
        {
            this.shapes = shapes;
            
            for (var i = 0; i < shapes.Length; i++)
            {
                shapes[i].GenerateRuntimeData();
                untrackedShapes.Add(i);
            }
        }
        
        private Shape[] shapes;
        
        private List<int> untrackedShapes = new List<int>();
        private List<TrackedShape> trackedShapes = new List<TrackedShape>();

        private Vector2 lastInput;

        public void Begin(Vector2 input) => lastInput = input;
        public IEnumerable<ShapeAnalysisResult> Evaluate(Vector2 input)
        {
            var settings = Repository.GetSingle<BeauTambourSettings>(Reference.Settings);
            var results = new List<ShapeAnalysisResult>();
            
            for (var i = 0; i < untrackedShapes.Count; i++)
            {
                if (!shapes[untrackedShapes[i]].CanStartEvaluation(input)) continue;
                
                OnEvaluationStart?.Invoke(shapes[untrackedShapes[i]]);
                
                trackedShapes.Add(new TrackedShape(i, lastInput));
                untrackedShapes.RemoveAt(i);
                i--;
            }

            for (var i = 0; i < trackedShapes.Count; i++)
            {
                var current = trackedShapes[i];
                var analysis = shapes[current.shapeIndex].Evaluate(current.analysis, input);
                
                if (analysis.GoToNext && analysis.index + 1 == shapes[current.shapeIndex].RuntimePoints.Count)
                {
                    untrackedShapes.Add(current.shapeIndex);
                    trackedShapes.RemoveAt(i);
                    i--;
                        
                    results.Add(new ShapeAnalysisResult(shapes[current.shapeIndex], analysis, true, false));
                    continue;
                }
                else
                {
                    if (analysis.Error >= settings.RecognitionErrorThreshold)
                    {
                        untrackedShapes.Add(current.shapeIndex);
                        trackedShapes.RemoveAt(i);
                        i--;

                        results.Add(new ShapeAnalysisResult(shapes[current.shapeIndex], analysis, false, true));
                        continue;
                    }
                    else results.Add(new ShapeAnalysisResult(shapes[current.shapeIndex], analysis, false, false));
                }

                current.analysis = analysis;
                trackedShapes[i] = current;
            }
            
            lastInput = input;
            return results;
        }
        public void Stop()
        {
            untrackedShapes.Clear();
            for (var i = 0; i < shapes.Length; i++) untrackedShapes.Add(i);
        
            trackedShapes.Clear();
        }
    }
}