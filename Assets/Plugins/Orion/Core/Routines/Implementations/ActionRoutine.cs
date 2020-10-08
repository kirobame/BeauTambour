using System;
using System.Collections;
using UnityEngine;

namespace Orion
{
    public class ActionRoutine : Routine
    {
        public Action action;
    
        protected override IEnumerator GetCall()
        {
            action();
            yield break;
        }
    }
}