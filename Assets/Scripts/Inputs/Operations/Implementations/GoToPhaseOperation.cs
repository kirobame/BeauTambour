using System;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewGoToPhaseOperation", menuName = "Beau Tambour/Operations/Go To Phase")]
    public class GoToPhaseOperation : PhaseBoundOperation
    {
        [SerializeField] private PhaseCategory destination;

        protected override void RelayedOnStart(EventArgs inArgs)
        {
            base.RelayedOnStart(inArgs);
            phaseHandler.Play(destination);
        }
    }
}