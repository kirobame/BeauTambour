using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Phase : MonoBehaviour, IPhase
{
    [SerializeField]private int length;
    [SerializeField]private PhaseType phaseType;
    private int actualBeats = 0;

    public event Action OnStart;
    public event Action OnEnd;

    public int Length {get => length; private set => length = value;}

    public PhaseType PhaseType { get => phaseType; private set => phaseType = value; }

    /// <summary>
    /// Manage phase progress
    /// </summary>
    /// <returns>TRUE if phase ended else FALSE</returns>
    public bool Advance()
    {
        if(actualBeats == 0)
        {
            Start();
        }
        actualBeats++;
        if (actualBeats >= Length)
        {
            End();
            return true;
        }
        return false;
    }

    public void End()
    {
        actualBeats = 0;
        OnEnd?.Invoke();
    }

    public void Start()
    {
        OnStart?.Invoke();
    }
}
