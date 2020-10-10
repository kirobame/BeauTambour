using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionHandler : MonoBehaviour
{
    private List<IBootable> bootables = new List<IBootable>();

    void Begin()
    {
        for (int indexBootables = 0; indexBootables < bootables.Count; indexBootables++)
        {
            bootables[indexBootables].BootUp();
        }
    }

    void End()
    {
        for (int indexBootables = 0; indexBootables < bootables.Count; indexBootables++)
        {
            bootables[indexBootables].ShutDown();
        }
    }
}
