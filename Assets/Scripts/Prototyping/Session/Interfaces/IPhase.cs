using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public interface IPhase
    {
        event Action OnStart;
        event Action OnEnd;
    
        PhaseType Type { get; }

        /// <summary>
        /// Manage phase progress
        /// </summary>
        /// <returns>TRUE if phase ended else FALSE</returns>
        bool Advance();
    
        void Begin();
        void End();    
    }
}