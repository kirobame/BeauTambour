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
        
        public void Begin(Shape shape, Vector2 input)
        {
            this.shape = shape;
            
            shapeAnalysis = new ShapeAnalysis(shape, lastInput);
            lastInput = input;
        }
        public ShapeAnalysis Evaluate(Vector2 input)
        {
            var analysis = shapeAnalysis;
            if (analysis.GoToNext && analysis.advancement == analysis.Source.RuntimePoints.Count - 1) analysis.advancement++;

            shapeAnalysis = analysis.Source.Evaluate(analysis, input);
            
            lastInput = input;
            return shapeAnalysis;
        }
        public void Stop() => shapeAnalysis = null;
    }
}