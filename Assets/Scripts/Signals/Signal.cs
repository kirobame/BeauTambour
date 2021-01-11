using System;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-2462194805134264945)]
    public abstract class Signal : ScriptableObject
    {
        public event Action OnEnd;

        public abstract string Category { get; }
        
        public Emotion Key => key;
        [SerializeField] private Emotion key;
        
        public abstract void Execute(MonoBehaviour hook, Character speaker, string[] args);

        protected void End() => OnEnd?.Invoke();
    }
}