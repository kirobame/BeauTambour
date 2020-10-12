using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public interface IBootable
    {
        int Priority { get; }
    
        void BootUp();
        void ShutDown();
    }
}