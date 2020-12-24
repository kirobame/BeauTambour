using System.Collections;
using System.Collections.Generic;
using Flux.Editor;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    [CustomPropertyDrawer(typeof(PhaseCallbackAddress))]
    public class PhaseCallbackAddressDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var labelRect = rect;
            labelRect.width = DrawingUtilities.labelWidth;
            
            EditorGUI.LabelField(labelRect, label);

            var addressRect = labelRect;
            addressRect.x += labelRect.width;
            addressRect.width = (rect.width - labelRect.width - DrawingUtilities.horizontalSpacing) * 0.5f;

            property.NextVisible(true);
            EditorGUI.PropertyField(addressRect, property, GUIContent.none);

            addressRect.x += addressRect.width + DrawingUtilities.horizontalSpacing;
            
            property.NextVisible(false);
            EditorGUI.PropertyField(addressRect, property, GUIContent.none);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;
    }
}