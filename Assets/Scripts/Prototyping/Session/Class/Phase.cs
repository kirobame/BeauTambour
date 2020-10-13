using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace BeauTambour.Prototyping
{
    public class Phase : MonoBehaviour, IPhase
    {
        public event Action OnStart;
        public event Action OnEnd;

        public PhaseType Type => type;

        public int Length => length;
        public int Advancement => advancement;
    
        [SerializeField] private int length;
        [SerializeField] private PhaseType type;
    
        private int advancement;

        /// <summary>
        /// Manage phase progress
        /// </summary>
        /// <returns>TRUE if phase ended else FALSE</returns>
        public virtual bool Advance()
        {
            advancement++;
            return advancement >= length;
        }
        public void Add(int value) => advancement += value;

        public virtual void Begin()
        {
            OnStart?.Invoke();
        }
        public virtual void End()
        {
            advancement = 0;
            OnEnd?.Invoke();
        }
    }
}