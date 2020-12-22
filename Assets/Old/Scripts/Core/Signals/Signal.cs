using System;
using Flux;
using UnityEngine;

namespace Deprecated
{
    [IconIndicator(-2462194805134264945)]
    public abstract class Signal : ScriptableObject
    {
        public event Action OnEnd;

        public abstract string Category { get; }
        
        public Emotion Key => key;
        public int Clarity => clarity;
        
        [SerializeField] private Emotion key;
        [SerializeField] private int clarity;

        public abstract void Execute(MonoBehaviour hook, Character character, string[] args);

        protected void End() => OnEnd?.Invoke();
    }
}