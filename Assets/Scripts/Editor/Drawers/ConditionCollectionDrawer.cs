using System;
using Flux.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BeauTambour.Editor
{
    [CustomCollection(typeof(Condition))]
    public class ConditionCollectionDrawer : EmbeddedScriptableCollectionDrawer<Condition>
    {
        public ConditionCollectionDrawer(SerializedObject serializedObject, SerializedProperty property) : base(serializedObject, property) { }
    }
}