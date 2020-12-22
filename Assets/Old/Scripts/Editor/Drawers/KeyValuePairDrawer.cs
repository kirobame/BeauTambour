using Flux.Editor;
using UnityEditor;
using UnityEngine;

namespace Deprecated.Editor
{
    [CustomPropertyDrawer(typeof(KeyValuePair))]
    public class KeyValuePairDrawer : FluxPropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;
            
            var width = rect.width - DrawingUtilities.horizontalSpacing + DrawingUtilities.indent;
            
            var partialRect = rect;
            partialRect.width = width * 0.5f;

            property.NextVisible(true);
            EditorGUI.PropertyField(partialRect, property, GUIContent.none);

            partialRect.x += partialRect.width + DrawingUtilities.horizontalSpacing - DrawingUtilities.indent;;

            property.NextVisible(false);
            EditorGUI.PropertyField(partialRect, property, GUIContent.none);

            EditorGUI.indentLevel = indentLevel;
        }
    }
}