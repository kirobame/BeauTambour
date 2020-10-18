using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Tooling
{
    public class StickSectorAction : SerializedMonoBehaviour, IBindable
    {
        [SerializeField] private Executable[] targets = new Executable[0];
        
        [SerializeField, Min(0.01f)] private float reactionTime;
        [SerializeField, Range(0f,1f)] private float wantedMagnitude;
        [SerializeField] private Vector2 limits;
        [SerializeField] private bool wrap;

        private Action<InputAction.CallbackContext> onPerformed;
        private Action<InputAction.CallbackContext> onCanceled;
        
        private float time = 0f;
        private bool hasBeenFired;

        protected virtual void Awake()
        {
            onPerformed = ctxt => Execute(ctxt.ReadValue<Vector2>());
            onCanceled = ctxt => { hasBeenFired = false; time = 0f; };
        }

        public void Execute(Vector2 position)
        {
            if (hasBeenFired) return;
            
            var angle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            var isInSector = false;
            if (!wrap) isInSector = angle >= limits.x && angle <= limits.y;
            else
            {
                var first = angle >= limits.x && angle <= 360f;
                var second = angle <= limits.y && angle >= 0f;

                isInSector = first || second;
            }

            if (isInSector)
            {
                if (position.magnitude >= wantedMagnitude && time <= reactionTime)
                {
                    foreach (var target in targets) target.Execute();
                    hasBeenFired = true;
                }

                time += Time.deltaTime;
            }
            else time = 0f;
        }
        
        void IBindable.Bind(InputAction inputAction)
        {
            inputAction.performed += onPerformed;
            inputAction.canceled += onCanceled;
        }
        void IBindable.Unbind(InputAction inputAction)
        {
            inputAction.performed -= onPerformed;
            inputAction.canceled -= onCanceled;
        }
    }
}