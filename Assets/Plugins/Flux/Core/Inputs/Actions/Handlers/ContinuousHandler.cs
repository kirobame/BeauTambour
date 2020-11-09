using UnityEngine;

namespace Flux
{
    public abstract class ContinuousHandler<T> : InputHandler<T>, IContinuousHandler
    {
        protected bool isActive;
        private T input;

        public override bool OnStarted(T input)
        {
            if (!base.OnStarted(input)) return false;
            
            isActive = true;
            this.input = input;

            return true;
        }
        public override bool OnPerformed(T input)
        {
            if (!base.OnPerformed(input)) return false;
            
            this.input = input;
            return true;
        }
        public override bool OnCanceled(T input)
        {
            if (!base.OnCanceled(input)) return false;
            
            this.input = input;
            isActive = false;
            
            return true;
        }

        void IContinuousHandler.Update()
        {
            if (!isActive) return;
            HandleInput(input);
        }
        protected abstract void HandleInput(T input);
    }
}