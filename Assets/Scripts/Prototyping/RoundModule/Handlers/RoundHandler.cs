using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour, IBootable
{
    private int actingLength;
    private int intersticeLength;
    private int resolutionLength;
    private State state;
    private List<IResolvable> resolvables = new List<IResolvable>();

    public Action OnActingStart { get; private set; }
    public Action OnActingEnd { get; private set; }

    public void BootUp()
    {
        throw new System.NotImplementedException();
    }

    public void ShutDown()
    {
        throw new System.NotImplementedException();
    }

    public bool TryEnqueue(IResolvable resolvable)
    {
        return false;
    }    
}
