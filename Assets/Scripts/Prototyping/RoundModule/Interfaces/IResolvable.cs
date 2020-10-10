using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResolvable 
{
    int Priority { get; set; }
    void Resolve();
}
