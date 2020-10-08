using System.Collections;
using UnityEngine;

namespace Orion
{
    public class InstructionRoutine : Routine
    {
        public object instruction;

        protected override IEnumerator GetCall() { yield return instruction; }
    }
}