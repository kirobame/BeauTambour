using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    /// <summary>
    /// An action that should take place during the Resolution phase.
    /// </summary>
    public interface IResolvable
    {
        int Priority { get; }
        void Resolve();
    }
}