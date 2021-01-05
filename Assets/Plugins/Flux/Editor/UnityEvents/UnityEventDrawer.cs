using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Flux.Editor
{
    [CustomPropertyDrawer(typeof(UnityEventBase), true)]
    public class UnityEventDrawer : PropertyDrawer
    {
        private bool hasBeenInitialized;
        private UnityEventAltDrawer drawer;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (!hasBeenInitialized) Initialize();

            EditorGUI.BeginProperty(rect, label, property);
            drawer.OnGUI(rect, property, label);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!hasBeenInitialized) Initialize();
            return drawer.GetPropertyHeight(property, label);
        }

        private void Initialize()
        {
            hasBeenInitialized = true;
            drawer = new UnityEventAltDrawer();
        }
    }
}