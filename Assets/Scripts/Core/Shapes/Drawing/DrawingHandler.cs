using System;
using System.Collections;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewDrawingHandler", menuName = "Beau Tambour/Handlers/Drawing")]
    public class DrawingHandler : InputHandler<Vector2>, IContinuousHandler
    {
        [SerializeField] private Shape[] shapes;
        [SerializeField] private float deactivationTime;
        
        private ShapeAnalyzer analyzer;
        
        private Vector2 input;
        private bool isActive;

        private Coroutine deactivationRoutine;
        
        public override void Initialize(OperationHandler handler)
        {
            base.Initialize(handler);
            
            analyzer = new ShapeAnalyzer(shapes);
            analyzer.OnEvaluationStart += shape => Begin(new ShapeEventArgs(shape));
        }
        
        protected override void OnStart(Vector2 input)
        {
            if (deactivationRoutine != null) handler.StopCoroutine(deactivationRoutine);
            
            isActive = true;
            this.input = input;
            
            analyzer.Begin(input);
        }
        protected override void OnPerformed(Vector2 input) => this.input = input;
        protected override void OnCanceled(Vector2 input)
        {
            this.input = input;
            deactivationRoutine = handler.StartCoroutine(DeactivationRoutine());
        }

        private IEnumerator DeactivationRoutine()
        {
            yield return  new WaitForSeconds(deactivationTime);
            
            deactivationRoutine = null;
            
            isActive = false;
            analyzer.Stop();
            
            End(new EventArgs());
        }

        void IContinuousHandler.Update()
        {
            if (isActive)
            {
                var results = analyzer.Evaluate(input);
                Prolong(new ShapeAnalyzerResultEventArgs(results));
            }
            else Prolong(new EventArgs());
        }
    }
}