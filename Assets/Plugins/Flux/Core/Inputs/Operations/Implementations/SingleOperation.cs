using System;
using UnityEngine;

namespace Flux
{
    public abstract class SingleOperation : Operation
    {
        protected IBindable bindable;
        
        public override void Bind(IBindable bindable)
        {
            if (isBinded) Unbind();
            this.bindable = bindable;

            bindable.onStart += OnStart;
            bindable.onUpdate += OnUpdate;
            bindable.onEnd += OnEnd;

            isBinded = true;
        }
        public override void Unbind()
        {
            if (bindable != null)
            {
                bindable.onStart -= OnStart;
                bindable.onUpdate -= OnUpdate;
                bindable.onEnd -= OnEnd;

                bindable = null;
            }

            isBinded = false;
        }
        
        public virtual void OnStart(EventArgs inArgs) { }
        public virtual void OnUpdate(EventArgs inArgs) { }
        public virtual void OnEnd(EventArgs inArgs) { }
    }
}