﻿using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class BumperModule : Module<bool>
    {
        [SerializeField] private ConfirmAction confirmAction;
        
        protected override void OnActionStarted(bool input) => confirmAction.TryBeginExecution();

        protected override void OnAction(bool input) { }
        protected override void OnActionEnded(bool input) { }
    }

    public class TriggerModule : Module<bool>
    {
        [SerializeField] private int sign;
        [SerializeField] private NoteAction noteAction;
        
        protected override void OnActionStarted(bool input) { }
        protected override void OnAction(bool input) { }
        
        protected override void OnActionEnded(bool input)
        {
            noteAction.sign = sign;
            noteAction.TryBeginExecution();
        }
    }
}