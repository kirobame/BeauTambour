using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionHandler : MonoBehaviour
{
    private List<IBootable> bootables = new List<IBootable>();

    public void Begin()
    {
        for (int indexBootables = 0; indexBootables < bootables.Count; indexBootables++)
        {
            bootables[indexBootables].BootUp();
        }
    }

    public void End()
    {
        for (int indexBootables = 0; indexBootables < bootables.Count; indexBootables++)
        {
            bootables[indexBootables].ShutDown();
        }
    }

    public void Subscribe(IBootable toAdd)
    {
        bootables.Add(toAdd);
    }
}
