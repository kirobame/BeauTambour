﻿using System;
using System.Collections;
using BeauTambour;
using Flux;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewDrawingHandler", menuName = "Beau Tambour/Inputs/Handlers/Drawing")]
    public class DrawingHandler : InputHandler<Vector2>, IContinuousHandler
    {
        [SerializeField] private Shape[] shapes;
        [SerializeField] private float activationTime;
        [SerializeField] private float deactivationTime;
        
        private ShapeAnalyzer analyzer;
        
        private Vector2 input;
        
        private bool isActive;
        private bool canAct;

        private Coroutine activationRoutine;
        private Coroutine deactivationRoutine;
        
        public override void Initialize(MonoBehaviour hook)
        {
            base.Initialize(hook);
            canAct = false;

            Event.Register(TempEvent.OnAnyMusicianPicked, () => activationRoutine = hook.StartCoroutine(ActivationRoutine()));
            Event.Register(DrawOperation.EventType.OnShapeMatch, () => canAct = false);
            
            analyzer = new ShapeAnalyzer(shapes);
            analyzer.OnEvaluationStart += shape => Begin(new ShapeEventArgs(shape));
        }
        private IEnumerator ActivationRoutine()
        {
            yield return new WaitForSeconds(activationTime);
            canAct = true;
        }
        
        public override bool OnStarted(Vector2 input)
        {
            if (!base.OnStarted(input) || !canAct) return false;
            if (deactivationRoutine != null) hook.StopCoroutine(deactivationRoutine);

            isActive = true;
            this.input = input;
            
            analyzer.Begin(input);
            return true;
        }
        public override bool OnPerformed(Vector2 input)
        {
            if (!base.OnPerformed(input)) return false;
            
            this.input = input;
            return true;
        }
        public override bool OnCanceled(Vector2 input)
        {
            if (!base.OnCanceled(input)) return false;
            
            this.input = input;
            deactivationRoutine = hook.StartCoroutine(DeactivationRoutine());

            return true;
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