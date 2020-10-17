using System;
using System.Linq;
using Orion;
using Shapes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Tooling
{
    public class Circle : Executable, IColorable
    {
        [SerializeField] private Disc disc;
        [SerializeField] private float targetRadius;
        [SerializeField] private float effectDuration;
        [SerializeField] private AnimationCurve curve;
        
        private float baseRadius;
        private Coroutine routine;

        protected override void Awake()
        {
            base.Awake();
            baseRadius = disc.Radius;
        }

        public override void Execute()
        {
            if (this.routine != null) StopCoroutine(this.routine);
            
            var routine = new TimedRoutine()
            {
                duration = effectDuration,
                during = time => disc.Radius = Mathf.Lerp(baseRadius, targetRadius, curve.Evaluate(time / effectDuration)),
                incrementTime = () => Time.deltaTime,
                waitInstruction = new WaitForEndOfFrame()
            };
            routine.Append(new ActionRoutine() {action = () =>
                {
                    disc.Radius = Mathf.Lerp(baseRadius, targetRadius, curve.Evaluate(1f));
                    this.routine = null;
                }
            });

            this.routine = StartCoroutine(routine.Call);
        }

        public void SetColor(Color color) => disc.Color = color;
    }
}