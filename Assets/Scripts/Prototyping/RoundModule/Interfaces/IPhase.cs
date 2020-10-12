using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhase
{
    PhaseType PhaseType { get; }
    event Action OnStart;
    event Action OnEnd;

    /// <summary>
    /// Manage phase progress
    /// </summary>
    /// <returns>TRUE if phase ended else FALSE</returns>
    bool Advance();
    void Start();
    void End();    
}