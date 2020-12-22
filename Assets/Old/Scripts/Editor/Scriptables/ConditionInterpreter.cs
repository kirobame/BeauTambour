using System;
using Flux;
using Flux.Editor;
using UnityEditor;
using UnityEngine;

namespace Deprecated.Editor
{
    public abstract class ConditionInterpreter<T> : OutcomeInterpreter where T : Condition
    {
        public override void Interpret(string data, Outcome outcome, Sequencer sequencer)
        {
            var condition = CreateInstance<T>();
            condition.hideFlags = HideFlags.HideInHierarchy;
            condition.name = $"{Guid.NewGuid()}-{typeof(T).Name}";
            
            AssetDatabase.AddObjectToAsset(condition, outcome);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(condition));
            
            HandleCondition(data, outcome, condition);
            
            var serializedObject = new SerializedObject(outcome);
            var conditionsProperty = serializedObject.FindProperty("conditions");
            
            conditionsProperty.AddElement(condition);
            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void HandleCondition(string data, Outcome outcome, T condition);
    }
}