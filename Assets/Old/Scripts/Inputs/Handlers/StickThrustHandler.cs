using System;
using Flux;
using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "NewStickThrustHandler", menuName = "Beau Tambour/Inputs/Handlers/Stick Thrust")]
    public class StickThrustHandler : ContinuousHandler<Vector2>
    {
        [Space, SerializeField] private Vector2 range;
        [SerializeField] private Vector2 thresholds;
        [SerializeField] private bool wrap;

        private bool state;

        protected override void HandleInput(Vector2 input)
        {
            var magnitude = input.magnitude;
            if (state == false)
            {
                if (magnitude < thresholds.x) return;
                
                var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
                if (angle < 0) angle += 360f;

                if (!wrap)
                {
                    if (angle >= range.x && angle <= range.y) Activate();
                }
                else  if (angle >= range.y || angle <= range.x) Activate();
            }
            else
            {
                if (magnitude > thresholds.y) return;
                state = false;
            }
        }

        private void Activate()
        {
            state = true;
            Begin(new EventArgs());
        }
    }
}