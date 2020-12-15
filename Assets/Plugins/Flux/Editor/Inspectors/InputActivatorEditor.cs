﻿using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flux.Editor
{
    [CustomEditor(typeof(InputActivator), true)]
    public class InputActivatorEditor : FluxEditor
    {
        protected override void Draw(SerializedProperty iterator)
        {
            iterator.NextVisible(false);
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(iterator);
            
            var asset = (InputActionAsset)iterator.objectReferenceValue;
            iterator.NextVisible(false);
            
            if (EditorGUI.EndChangeCheck()) Refresh(iterator, asset);

            for (var i = 0; i < iterator.arraySize; i++)
            {
                var elementProperty = iterator.GetArrayElementAtIndex(i);
                
                elementProperty.NextVisible(true);
                var label = new GUIContent(elementProperty.stringValue);

                elementProperty.NextVisible(false);
                EditorGUILayout.PropertyField(elementProperty, label);
            }

            if (GUILayout.Button("Refresh")) Refresh(iterator, asset);
        }

        private void Refresh(SerializedProperty iterator, InputActionAsset asset)
        {
            iterator.arraySize = 0;
            if (asset != null)
            {
                for (var i = 0; i < asset.actionMaps.Count; i++)
                {
                    iterator.InsertArrayElementAtIndex(0);
                    var elementProperty = iterator.GetArrayElementAtIndex(0);

                    elementProperty.NextVisible(true);
                    elementProperty.stringValue = asset.actionMaps[i].name;

                    elementProperty.NextVisible(false);
                    elementProperty.boolValue = true;
                }
            }
        }
    }
}