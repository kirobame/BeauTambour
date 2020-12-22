using System;
using Flux.Editor;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Deprecated.Editor
{
     [CustomCollection(typeof(Condition))]
     public class ConditionCollectionDrawer : EmbeddedScriptableCollectionDrawer<Condition>
     {
         public ConditionCollectionDrawer(SerializedObject serializedObject, SerializedProperty property) : base(serializedObject, property) { }
     }
}