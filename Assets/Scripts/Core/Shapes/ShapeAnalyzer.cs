using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public class ShapeAnalyzer
    {
        #region Encapsulated Types

        private struct TrackedShape
        {
            public TrackedShape(int index)
            {
                this.index = index;

                advancement = 0;
                error = 0;
            }

            public readonly int index;

            public int advancement;
            public float error;
        }
        #endregion

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

        public bool Evaluate(Vector2 input, out Shape result)
        {
            for (var i = 0; i < untrackedShapes.Count; i++)
            {
                if (!shapes[untrackedShapes[i]].CanStartEvaluation(input)) continue;
                
                trackedShapes.Add(new TrackedShape(untrackedShapes[i]));
                untrackedShapes.RemoveAt(i);
                i--;
            }

            var direction = (input - lastInput).normalized;
            for (var i = 0; i < trackedShapes.Count; i++)
            {
                var current = trackedShapes[i];
                var error = shapes[current.index].Evaluate(input, direction, current.advancement, out var next);

                if (next)
                {
                    if (current.advancement + 1 == shapes[current.index].RuntimePoints.Count - 1)
                    {
                        untrackedShapes.Add(current.index);
                        trackedShapes.RemoveAt(i);
                        i--;
                        
                        result = shapes[current.index];
                        return true;
                    }
                    else
                    {
                        current.error = 0;
                        current.advancement++;

                        trackedShapes[i] = current;
                    }
                }
                else
                {
                    current.error += error;
                    if (current.error > 10)
                    {
                        untrackedShapes.Add(current.index);
                        trackedShapes.RemoveAt(i);
                        i--;
                    }
                    else trackedShapes[i] = current;
                }
            }
            lastInput = input;

            result = null;
            return false;
        }
        public void Stop()
        {
            untrackedShapes.Clear();
            for (var i = 0; i < shapes.Length; i++) untrackedShapes.Add(i);
        
            trackedShapes.Clear();
        }
    }
}