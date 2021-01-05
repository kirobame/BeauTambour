using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour
{
    public abstract class AlphaTarget : MonoBehaviour
    {
        protected float goal;

        public virtual void Prepare(float goal) => this.goal = goal;
        public abstract void Set(float ratio);
    }
}