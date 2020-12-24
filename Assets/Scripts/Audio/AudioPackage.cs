using System;
using System.Collections;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-4387529911086606607)]
    public abstract class AudioPackage : ScriptableObject
    {
        public abstract void AssignTo(AudioSource source, EventArgs inArgs);
    }
}