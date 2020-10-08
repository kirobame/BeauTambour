using System;
using System.Linq;
using Ludiq.OdinSerializer.Utilities;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Orion.Editor
{
    [CustomEditor(typeof(FeedbackPlayer))]
    public class FeedbackPlayerEditor : OdinEditor
    {
        private GenericSelector<Type> dropdown;
        private Type type;

        protected override void OnEnable()
        {
            var types = typeof(Feedback).GetDependencies().ToArray();
            
            dropdown = new GenericSelector<Type>(string.Empty, false, GetEffectName, types);
            dropdown.SelectionTree.DefaultMenuStyle.Height = (int) EditorGUIUtility.singleLineHeight + 4;
            dropdown.SetSelection(types.First());

            dropdown.SelectionChanged += items => type = items.FirstOrDefault();
            dropdown.SelectionConfirmed += items => type = items.FirstOrDefault();
        }
        
        private string GetEffectName(Type type) => type.Name.Replace("Effect", string.Empty);
        
        protected override void DrawTree()
        {
            EditorGUILayout.BeginHorizontal();

            var size = EditorGUIUtility.singleLineHeight;
            if (GUILayout.Button(type == null ? " None" : $" {GetEffectName(type)}", EditorStyles.popup))
            {
                var rect = GUIHelper.GetCurrentLayoutRect();
                var position = new Vector2(rect.x, rect.yMax);
                
                var window = dropdown.ShowInPopup(position, rect.width - size - 3f);
                window.OnClose += dropdown.SelectionTree.Selection.ConfirmSelection; 
            }

            if (GUILayout.Button(EditorIcons.Plus.ActiveGUIContent, SirenixGUIStyles.IconButton, GUILayout.Width(size), GUILayout.Height(size)))
            {
                if (type == null) return;
                var component = target as Component;
                
                var addedComponent = component.gameObject.AddComponent(type);
                addedComponent.hideFlags = HideFlags.HideInInspector;

                var feedbacksProperty = serializedObject.FindProperty("feedbacks");
                var index = feedbacksProperty.arraySize;
                feedbacksProperty.InsertArrayElementAtIndex(index);

                var element = feedbacksProperty.GetArrayElementAtIndex(index);
                element.objectReferenceValue = addedComponent;
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(size / 3f);
            
            Tree.DrawMonoScriptObjectField = false;
            InspectorUtilities.BeginDrawPropertyTree(Tree, true);
            
            InspectorUtilities.DrawPropertiesInTree(Tree);
            
            InspectorUtilities.EndDrawPropertyTree(Tree);
        }
    }
}