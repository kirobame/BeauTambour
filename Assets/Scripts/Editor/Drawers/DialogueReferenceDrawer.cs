using Flux.Editor;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    [CustomPropertyDrawer(typeof(DialogueReference))]
    public class DialogueReferenceDrawer : PropertyDrawer
    {
        private bool hasBeenInitialized;
        private RuntimeSettings runtimeSettings;

        private Vector2 scrollPosition;
        
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (!hasBeenInitialized) Initialize();
            var sourceProperty = property.Copy();
            
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            
            var labelRect = rect;
            labelRect.width = DrawingUtilities.labelWidth;

            EditorGUI.LabelField(labelRect, label);
            var fieldWidth = DrawingUtilities.fieldWidth - EditorGUIUtility.singleLineHeight - DrawingUtilities.horizontalSpacing * 2f;

            var indexRect = labelRect;
            indexRect.x += labelRect.width;
            indexRect.width = fieldWidth * 0.25f;

            property.NextVisible(true);
            EditorGUI.PropertyField(indexRect, property, GUIContent.none);

            var encounterIndex = property.intValue;

            property.NextVisible(false);
            var id = property.stringValue;

            var match = BeauTambourUtilities.DialogueProvider.TryGetDialogue(encounterIndex - 1, id, runtimeSettings.Language, out var dialogue);

            var baseGuiColor = GUI.color;
            if (match) GUI.color = new Color(0.4f,0.85f,0.35f, 0.75f);
            else
            {
                GUI.color = new Color(0.95f,0.4f,0.4f, 0.75f);
                property.isExpanded = false;
            }
            
            var idRect = indexRect;
            idRect.x += indexRect.width + DrawingUtilities.horizontalSpacing;
            idRect.width = fieldWidth * 0.75f;

            EditorGUI.PropertyField(idRect, property, GUIContent.none);
            GUI.color = baseGuiColor;

            var buttonRect = idRect;
            buttonRect.x += idRect.width + DrawingUtilities.horizontalSpacing;
            buttonRect.width = EditorGUIUtility.singleLineHeight;

            if (!match)
            {
                GUI.enabled = false;
                sourceProperty.isExpanded = false;

                var content = EditorGUIUtility.IconContent("animationvisibilitytoggleoff@2x");
                GUI.Button(buttonRect, content, EditorStyles.label);
                
                GUI.enabled = true;
            }
            else
            {
                var content = sourceProperty.isExpanded ? EditorGUIUtility.IconContent("animationvisibilitytoggleon@2x") : EditorGUIUtility.IconContent("animationvisibilitytoggleoff@2x");
                if (GUI.Button(buttonRect, content, EditorStyles.label)) sourceProperty.isExpanded = !sourceProperty.isExpanded;

                if (!sourceProperty.isExpanded) return;
                
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;

                var frame = rect;
                frame.height = EditorGUIUtility.singleLineHeight * 3f;
                frame.width -= 4f;
                
                var style = new GUIStyle(EditorStyles.textArea);
                var textContent = new GUIContent(dialogue.ToString());

                rect.height = style.CalcHeight(textContent, EditorGUIUtility.currentViewWidth);
                rect.width -= DrawingUtilities.horizontalSpacing + EditorGUIUtility.singleLineHeight;
                
                scrollPosition = GUI.BeginScrollView(frame, scrollPosition, rect);

                GUI.enabled = false;
                EditorGUI.TextArea(rect, dialogue.ToString());
                GUI.enabled = true;
                
                GUI.EndScrollView(true);
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!hasBeenInitialized) Initialize();

            if (!property.isExpanded) return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
            else
            {
                return EditorGUIUtility.singleLineHeight * 4f + EditorGUIUtility.standardVerticalSpacing * 4f;
            }
        }

        private void Initialize()
        {
            runtimeSettings = AssetDatabase.LoadAssetAtPath<RuntimeSettings>("Assets/Settings/RuntimeSettings.asset");
            hasBeenInitialized = true;
        }
    }
}