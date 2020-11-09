using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flux.Editor
{
    [CustomPropertyDrawer(typeof(InputMapReference))]
    public class InputMapReferenceDrawer : PropertyDrawer
    {
        private bool hasBeenInitialized;
        private string[] options;
        private int selection;
        
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            if (!hasBeenInitialized) Initialize(property);

            rect.y += EditorGUIUtility.standardVerticalSpacing;
            rect.height -= EditorGUIUtility.standardVerticalSpacing;

            var labelRect = rect;
            labelRect.width = DrawingUtilities.labelWidth;
            
            EditorGUI.LabelField(labelRect, label);

            property.NextVisible(true);
            var width = DrawingUtilities.fieldWidth - DrawingUtilities.horizontalSpacing;
            
            var assetRect = labelRect;
            assetRect.x += labelRect.width;
            assetRect.width = width * 0.5f;

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(assetRect, property, GUIContent.none);

            if (EditorGUI.EndChangeCheck())
            {
                TryFetchOptions(property);
                selection = 0;
            }

            if (!options.Any()) GUI.enabled = false;
            property.NextVisible(false);
            
            var optionsRect = assetRect;
            optionsRect.x += assetRect.width + DrawingUtilities.horizontalSpacing;

            selection = EditorGUI.Popup(optionsRect, selection, options);
            if (options.Any()) property.stringValue = options[selection];

            GUI.enabled = true;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!hasBeenInitialized)
            {
                Initialize(property);

                property.NextVisible(true);
                property.NextVisible(false);

                var option = property.stringValue;
                var index = options.IndexOf(option);

                if (index != -1) selection = index;
            }
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
        }

        private void Initialize(SerializedProperty property)
        {
            var copy = property.Copy();
            
            copy.NextVisible(true);
            TryFetchOptions(copy);
            
            hasBeenInitialized = true;
        }
        private void TryFetchOptions(SerializedProperty property)
        {
            var asset = (InputActionAsset)property.objectReferenceValue;
            if (asset != null)
            {
                options = new string[asset.actionMaps.Count];
                for (var i = 0; i < options.Length; i++) options[i] = asset.actionMaps[i].name;
            }
            else options = new string[0];

            selection = 0;
        }
    }
}