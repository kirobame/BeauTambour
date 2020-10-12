﻿using Orion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class RoundHandler : SerializedMonoBehaviour, IBootable
    {
        public event Action OnRoundLoop;
        
        //--------------------------------------------------------------------------------------------------------------
        
        public PhaseType CurrentType { get; private set; }
    
        //--------------------------------------------------------------------------------------------------------------

        [SerializeField] private int bootUpPriority;
    
        [Space, SerializeField] private List<IPhase> phases = new List<IPhase>();
        private int advancement = -2;

        //--------------------------------------------------------------------------------------------------------------

        int IBootable.Priority => bootUpPriority;
    
        //--------------------------------------------------------------------------------------------------------------

        void Start() => Repository.Get<RythmHandler>().OnBeat += OnBeat;

        public void BootUp()
        {
            advancement = 0;
            phases.First().Begin();
        }
        public void ShutDown() => advancement = -2;

        private void OnBeat(double beat)
        {
            var shouldMoveToNext = phases[advancement].Advance();
            if (shouldMoveToNext)
            {
                phases[advancement].End();

                if (advancement + 1 >= phases.Count)
                {
                    advancement = 0;
                    OnRoundLoop?.Invoke();
                }
                else advancement++;
           
                phases[advancement].Begin();
            }
        }
    }
}