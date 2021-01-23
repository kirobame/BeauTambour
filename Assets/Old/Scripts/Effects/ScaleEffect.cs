﻿using BeauTambour;
using Flux;
using UnityEngine;

namespace Deprecated
{
    public class ScaleEffect : InterpolationEffect
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 start;
        [SerializeField] private Vector3 end;

        protected override void Startup()
        {
            throw new System.NotImplementedException();
        }

        protected override void Execute(float ratio)
        {
            target.localScale = Vector3.Lerp(start, end, ratio);
        }

        protected override void End()
        {
            throw new System.NotImplementedException();
        }
    }
}