using Deprecated;
using Flux;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public class ShapeAnalyzer
    {
        private Shape shape;
        private ShapeAnalysis shapeAnalysis;

        private Vector2 lastInput;

        public event Action<Shape> OnEvaluationStart;

        public ShapeAnalyzer(Shape shape)
        {
            this.shape = shape;

            shape.GenerateRuntimePoints();            
        }


        public void Begin(Vector2 input) => lastInput = input;

        public ShapeAnalysis Evaluate(Vector2 input)
        {
            if (shapeAnalysis == null)
            {
                OnEvaluationStart?.Invoke(shape);
                shapeAnalysis = new ShapeAnalysis(shape, lastInput);
            }
            var settings = Repository.GetSingle<BeauTambourSettings>(Reference.Settings);

            var analysis = shapeAnalysis;
            if (analysis.GoToNext && analysis.advancement == analysis.Source.RuntimePoints.Count - 1)
            {
                analysis.advancement++;
            }
            else if (analysis.Error >= settings.RecognitionErrorThreshold)
            {
                //End
            }

            shapeAnalysis = analysis.Source.Evaluate(analysis, input);
            

            lastInput = input;
            return shapeAnalysis;
        }
        public void Stop()
        {
            /*untrackedShapes.Clear();
            for (var i = 0; i < shapes.Length; i++) untrackedShapes.Add(i);

            trackedShapes.Clear();*/
            //shapeAnalysis = null;
        }
    }
}