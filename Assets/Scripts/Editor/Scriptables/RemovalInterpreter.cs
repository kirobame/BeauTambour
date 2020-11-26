using Flux;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    [CreateAssetMenu(fileName = "NewRemovalInterpreter", menuName = "Beau Tambour/Editor/Outcome Interpreters/Should Be Removed")]
    public class RemovalInterpreter : OutcomeInterpreter
    {
        public override void Interpret(string data, Outcome outcome, Sequencer sequencer)
        {
            var serializedObject = new SerializedObject(outcome);
            var removalProperty = serializedObject.FindProperty("shouldBeRemoved");

            removalProperty.boolValue = !bool.Parse(data);
            serializedObject.ApplyModifiedProperties();
        }
    }
}