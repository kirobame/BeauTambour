using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    public abstract class Effect : MonoBehaviour
    {
        public bool IsDone { get; protected set; }
        
        public int Index => index;
        private int index;

        public void Bootup(int index) => this.index = index;
        public virtual void Initialize() => IsDone = false;

        public virtual int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            IsDone = true;
            
            prolong = true;
            return advancement + 1;
        }
    }
}