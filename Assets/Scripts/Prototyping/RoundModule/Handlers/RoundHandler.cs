using BeauTambour.Prototyping;
using Orion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class RoundHandler : SerializedMonoBehaviour, IBootable
{
    public PhaseType Current => actualPhaseType;
    
    [SerializeField] private List<IPhase> phases;
    
    [ShowInInspector, ReadOnly] private PhaseType actualPhaseType;
    private int actualPhaseIndex = 0;
    [SerializeField] private int indexReso;

    private List<IResolvable> resolvables = new List<IResolvable>();

    private void Start()
    {
        Repository.Get<RythmHandler>().OnBeat += OnBeated;
        SubscribePhasesEvents();
        phases[indexReso].OnStart += HandleResolvable;
    }

    #region BOOTABLE
    public void BootUp()
    {
        resolvables = new List<IResolvable>();
        actualPhaseType = phases.First().PhaseType;      
    }

    public void ShutDown()
    {
        resolvables.Clear();
        resolvables = null;
        phases.Clear();
        phases = null;
    }
    #endregion

    private void HandleResolvable()
    {
        foreach (IResolvable resolvable in resolvables)
        {
            resolvable.Resolve();
        }
        resolvables.Clear();
    }

    /// <summary>
    /// Add an IResolvable object in the priority Queue (high priority -> used in first)
    /// </summary>
    /// <param name="resolvable">object to add</param>
    /// <returns>TRUE if it worked else FALSE</returns>
    public bool TryEnqueue(IResolvable resolvable)
    {
        if(resolvable == null)
        {
            return false;
        }
        resolvables.Add(resolvable);
        resolvables.Sort((resolvable1, resolvable2)=> { 
                return resolvable1.Priority.CompareTo(resolvable2.Priority); 
            }
        );
        return true;
    }

    /// <summary>
    /// Subscribe to Start and End events of phases
    /// </summary>
    private void SubscribePhasesEvents()
    {
        foreach (IPhase phase in phases)
        {
            phase.OnStart += OnPhaseStarted;
            phase.OnEnd += OnPhaseEnded;
        }
    }

    /// <summary>
    /// Reset the roundHandler to start a new round
    /// </summary>
    private void ResetRound()
    {
        actualPhaseIndex = 0;
        actualPhaseType = PhaseType.Start;
    }

    #region CALLBACKS

    /// <summary>
    /// Called when beat lauched
    /// </summary>
    /// <param name="beat">actual number of beats in the whole session</param>
    private void OnBeated(double beat)
    {
        phases[actualPhaseIndex].Advance();
        switch (actualPhaseType)
        {
            case PhaseType.Acting:
                //Déroulement
                Debug.Log("Acting");
                break;
            case PhaseType.Interstice:
                //Déroulement
                Debug.Log("Inter");
                break;
            case PhaseType.Resolution:
                //Déroulement
                Debug.Log("Resolution");
                break;
            case PhaseType.End:
                Debug.Log("Ended");
                ResetRound();                
                break;
        }
    }

    /// <summary>
    /// Called when a phase start
    /// </summary>
    private void OnPhaseStarted()
    {
        actualPhaseType = phases[actualPhaseIndex].PhaseType;
    }

    /// <summary>
    /// Called when a phase end
    /// </summary>
    private void OnPhaseEnded()
    {
        actualPhaseIndex++;
        if (actualPhaseIndex >= phases.Count)
        {
            actualPhaseType = PhaseType.End;
            return;
        }
    }
    #endregion
}
