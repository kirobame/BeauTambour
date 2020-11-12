using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cue
{    
    private string text;
    private ActorTest actor;      

    public string Text => text;
    public ActorTest Actor => actor;


    public Cue(string p_text, ActorTest p_actor)
    {
        text = p_text;
        actor = p_actor;
    }

    /// <summary>
    /// Check if 2 Cues have the same Actor.
    /// </summary>
    /// <param name="other">The other actor.</param>
    /// <returns>TRUE : same actor
    /// FALSE : not the same
    /// </returns>
    public bool HasSameActorAs(Cue other)
    {
        return actor.Equals(other.actor);
    }    
}
