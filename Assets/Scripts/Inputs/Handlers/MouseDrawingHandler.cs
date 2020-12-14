using System;
using System.Collections;
using Flux;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewMouseDrawingHandler", menuName = "Beau Tambour/Inputs/Handlers/Mouse Drawing")]
    public class MouseDrawingHandler : InputHandler<Vector2>, IContinuousHandler
    {
        [SerializeField] private Shape[] shapes;
        [SerializeField] private float deactivationTime;
        
        private ShapeAnalyzer analyzer;
        
        private float radius;
        private Vector2 center;
        
        private Vector2 input;
        
        private bool isActive;
        private Coroutine deactivationRoutine;

        private bool canAct;
        private bool isMatch;

        void Setup()
        {
            canAct = true;
            
            if (!DeviceAlternationHandler.IsMouse) return;

            isActive = true;
            OnStarted(Vector2.zero);
        }

        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            canAct = false;

            phase = Phase.Canceled;
            isActive = false;

            Event.Register(TempEvent.OnAnyMusicianPicked, Setup);
            Event.Register(DrawOperation.EventType.OnShapeLoss, () => isMatch = false);
            Event.Register(DrawOperation.EventType.OnShapeMatch, () =>
            {
                isMatch = true;
                canAct = false;
            });
            
            analyzer = new ShapeAnalyzer(shapes);
            analyzer.OnEvaluationStart += shape => Begin(new ShapeEventArgs(shape));
            
            var drawingPool = Repository.GetSingle<DrawingPool>(Pool.Drawing);
            var camera = Repository.GetSingle<Camera>(Reference.Camera);
            
            center = camera.WorldToScreenPoint(drawingPool.transform.position);

            var offset = drawingPool.transform.position + Vector3.right * drawingPool.transform.localScale.x;
            radius = Vector2.Distance(center, camera.WorldToScreenPoint(offset));
        }
        
        public override bool OnStarted(Vector2 input)
        {
            if (phase != Phase.Canceled || !isActive || !canAct) return false;
            if (deactivationRoutine != null) hook.StopCoroutine(deactivationRoutine);

            isMatch = false;
            
            analyzer.Begin(input);
            phase = Phase.Started;
            
            return true;
        }
        public override bool OnPerformed(Vector2 input)
        {
            if (!base.OnPerformed(input)) return false;
     
            ComputeInput(input);
            return true;
        }
        public override bool OnCanceled(Vector2 input)
        {
            if (!base.OnCanceled(input)) return false;

            this.input = Vector2.zero;
            deactivationRoutine = hook.StartCoroutine(DeactivationRoutine());

            return true;
        }
        
        private IEnumerator DeactivationRoutine()
        {
            yield return  new WaitForSeconds(deactivationTime);
            deactivationRoutine = null;
            
            analyzer.Stop();
            isActive = true;
            
            End(new EventArgs());

            if (!isMatch) OnStarted(Vector2.zero);
            else isActive = false;
        }
        void IContinuousHandler.Update()
        {
            if (isActive)
            {
                var results = analyzer.Evaluate(this.input);
                Prolong(new ShapeAnalyzerResultEventArgs(results));
            }
            else Prolong(new EventArgs());
        }

        private void ComputeInput(Vector2 input)
        {
            this.input = (input - center) / radius;
            if (this.input.magnitude > 1) this.input.Normalize();
        }
    }
}