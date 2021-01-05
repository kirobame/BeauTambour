using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Flux.Editor
{
    [CustomPropertyDrawer(typeof(EventTrigger.TriggerEvent))]
    public class TriggerEventDrawer : PropertyDrawer
    {
        private Dictionary<string, UnityEventAltDrawer> registry = new Dictionary<string, UnityEventAltDrawer>();

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (!registry.ContainsKey(property.propertyPath)) Initialize(property.Copy());

            EditorGUI.BeginProperty(rect, label, property);
            registry[property.propertyPath].OnGUI(rect, property, label);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!registry.ContainsKey(property.propertyPath)) Initialize(property.Copy());
            return registry[property.propertyPath].GetPropertyHeight(property, new GUIContent("AAA"));
        }

        private void Initialize(SerializedProperty property)
        {
            var path = property.propertyPath;
            var lastIndex = path.LastIndexOf('.');
            var parentPath = path.Remove(lastIndex);
            
            var baseProperty = property.serializedObject.FindProperty(parentPath);
            var copy = baseProperty.Copy();
            copy.Next(true);

            var drawer = new UnityEventAltDrawer();
            drawer.SetFallbackName(((EventTriggerType)copy.enumValueIndex).ToString());
            
            registry.Add(property.propertyPath, drawer);
        }
    }
}