﻿using UnityEngine;

namespace Flux
{
    public abstract class Poolable : MonoBehaviour { }
    public abstract class Poolable<T> : Poolable
    {
        public Poolable<T> Key => key;
        
        public T Value => value;
        [SerializeField] private T value;
        
        private Poolable<T> key;
        protected Pool<T> origin;

        protected bool hasBeenBootedUp;

        protected virtual void Awake()
        { 
            if (!hasBeenBootedUp) Bootup();
        }
        protected virtual void Bootup() { }
        
        protected virtual void OnDisable()
        {
            if (!hasBeenBootedUp) hasBeenBootedUp = true;
            else origin.Stock(this);
        }
            
        public void SetOrigin(Pool<T> origin, Poolable<T> key)
        {
            this.key = key;
            this.origin = origin;
        }
        public virtual void Prepare() { }

        public void BypassBootup()
        {
            Bootup();
            hasBeenBootedUp = true;
        }
        public virtual void Reboot() { }
    }
}