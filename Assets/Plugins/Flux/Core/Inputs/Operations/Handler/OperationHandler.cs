﻿using System.Linq;
using UnityEngine;

namespace Flux
{
    public class OperationHandler : MonoBehaviour
    {
        [SerializeField] private InputLink[] inputLinks;
        
        private IContinuousHandler[] continuousHandlers;
        
        void Start()
        {
            foreach (var inputLink in inputLinks) inputLink.Initialize(this);
            continuousHandlers = inputLinks.SelectMany(link => link.Handlers).Where(handler => handler is IContinuousHandler).Cast<IContinuousHandler>().ToArray();
        }

        void Update() { foreach (var continuousHandler in continuousHandlers) continuousHandler.Update(); }
        
        public void Shutdown() { foreach (var inputLink in inputLinks) inputLink.Shutdown(); }
    }
}