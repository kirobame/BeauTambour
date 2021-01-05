using System;
using UnityEngine;

namespace BeauTambour
{
    public abstract class DialogueEvent : ScriptableObject
    {
        public Action OnEnd;
        
        public string Key => key;
        [SerializeField] private string key;

        public abstract void Execute(MonoBehaviour hook);
        protected void End() => OnEnd?.Invoke();
    }
}