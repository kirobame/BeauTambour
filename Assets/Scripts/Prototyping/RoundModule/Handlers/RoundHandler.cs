using BeauTambour.Prototyping;
using Orion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour, IBootable
{
    private const int actingLength = 8;
    private const int intersticeLength = 8;
    private const int resolutionLength = 8;

    private RythmHandler rythmHandler;
    [SerializeField]private Token rythmHandlerToken;
    private double beatsPerSeconds = 1.25f;
    private State state;
    private List<IResolvable> resolvables;
    private bool roundEnded = false;
    private int roundBeats = 0;

    public event Action onActingStart;

    public event Action onActingEnd;

    private void Start()
    {
        rythmHandler = Repository.Get<RythmHandler>(rythmHandlerToken);
        rythmHandler.OnBeat += OnBeated;
    }

    public void RoundProgress()
    {
        roundBeats = 0;
        do
        {      
            // Waiting for Beat -> see OnBeated
        } while (!roundEnded);
    }

    public void BootUp()
    {
        resolvables = new List<IResolvable>();
        state = State.Acting;
        roundEnded = false;        
    }

    public void ShutDown()
    {
        resolvables.Clear();
        resolvables = null;
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
    /// Subscribe to a callback launched at the acting's start
    /// </summary>
    /// <param name="toDo">the action that the callback have to do</param>
    public void ActingStartSubscribe(Action toDo)
    {
        onActingStart += toDo;
    }
    public void ActingdEndSubscribe(Action toDo)
    {
        onActingEnd-= toDo;
    }

    private void PhaseChanging(int beats)
    {
        if(beats >= actingLength + intersticeLength + resolutionLength)
        {
            state = State.End;
        }
        else if(beats >= actingLength + intersticeLength)
        {
            state = State.Resolution;
        }
        else if(beats >= actingLength )
        {
            state = State.Interstice;
        }
    }

    private void OnBeated(double beat)
    {
        roundBeats++;
        PhaseChanging(roundBeats);
        switch (state)
        {
            case State.Acting:
                //Déroulement
                break;
            case State.Interstice:
                //Déroulement
                break;
            case State.Resolution:
                //Déroulement
                break;
            case State.End:
                roundEnded = true;
                break;
        }
    }
}
