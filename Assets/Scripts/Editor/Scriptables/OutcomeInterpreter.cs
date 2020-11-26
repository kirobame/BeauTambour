using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

namespace BeauTambour.Editor
{
    public abstract class OutcomeInterpreter : ScriptableObject
    {
        [SerializeField] protected string key;

        public void TryFor(string header, Outcome outcome, Sequencer sequencer)
        {
            var start = header.IndexOf(key, StringComparison.Ordinal);
            if (start == -1) return;

            start += key.Length;
            var end = header.IndexOf(';', start);
            
            var data = header.Substring(start, end - start);
            Interpret(data, outcome, sequencer);
        }

        public abstract void Interpret(string data, Outcome outcome, Sequencer sequencer);
    }
}