using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Flux.Editor
{
    [CustomCollection(typeof(Effect))]
    public class EffectSequenceDrawer : EmbeddedCollectionDrawer<Effect> 
    {
        public EffectSequenceDrawer(SerializedObject serializedObject, SerializedProperty property) : base(serializedObject, property) { }

        protected override Effect CreateItem(Type itemType)
        {
            var root = reorderableList.serializedProperty.serializedObject.targetObject as Component;
            var effect = (Effect)root.gameObject.AddComponent(itemType);

            effect.hideFlags = HideFlags.HideInInspector;
            return effect;
        }
        protected override void DeleteItem(Effect item) => Object.DestroyImmediate(item, true);
    }
}