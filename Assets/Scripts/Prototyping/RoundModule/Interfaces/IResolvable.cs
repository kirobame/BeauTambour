using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An action that should take place at the round's end
/// </summary>
public interface IResolvable
{
    int Priority { get; set; }
    void Resolve();
}
