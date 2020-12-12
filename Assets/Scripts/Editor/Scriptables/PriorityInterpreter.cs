using Flux;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    [CreateAssetMenu(fileName = "NewPriorityInterpreter", menuName = "Beau Tambour/Editor/Outcome Interpreters/Priority")]
    public class PriorityInterpreter : OutcomeInterpreter
    {
        public override void Interpret(string data, Outcome outcome, Sequencer sequencer)
        {
            var serializedObject = new SerializedObject(outcome);
            var priorityProperty = serializedObject.FindProperty("priority");
            
            var priority = int.Parse(data);
            priorityProperty.intValue = priority;

            serializedObject.ApplyModifiedProperties();
        }
    }
}