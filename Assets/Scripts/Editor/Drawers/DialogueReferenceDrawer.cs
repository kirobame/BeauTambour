using Flux;
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

        private bool isExpanded;
        private Vector2 scrollPosition;
        
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            
            if (!hasBeenInitialized) Initialize();
            
            rect.height = EditorGUIUtility.singleLineHeight;
            //rect.y += EditorGUIUtility.standardVerticalSpacing;
            
            var labelRect = rect;
            labelRect.width = DrawingUtilities.labelWidth - DrawingUtilities.indent + 9f;

            EditorGUI.LabelField(labelRect, label);
           
            var fieldWidth = DrawingUtilities.fieldWidth - DrawingUtilities.horizontalSpacing * 3f - EditorGUIUtility.singleLineHeight - DrawingUtilities.indent * 2f - 14f;
            
            var indexRect = labelRect;
            indexRect.x += labelRect.width + DrawingUtilities.horizontalSpacing - DrawingUtilities.indent;
            indexRect.width = fieldWidth * 0.25f + DrawingUtilities.indent;

            property.NextVisible(true);
            EditorGUI.PropertyField(indexRect, property, GUIContent.none);

            var encounterId = property.stringValue;
            property.NextVisible(false);
            var id = property.stringValue;

            var match = BeauTambourUtilities.DialogueProvider.TryGetDialogue(encounterId, id, runtimeSettings.Language, out var dialogue);

            var baseGuiColor = GUI.color;
            if (match) GUI.color = new Color(0.4f,0.85f,0.35f, 0.75f);
            else
            {
                GUI.color = new Color(0.95f,0.4f,0.4f, 0.75f);
                property.isExpanded = false;
            }
            
            var idRect = indexRect;
            idRect.x += indexRect.width + EditorGUIUtility.standardVerticalSpacing - DrawingUtilities.indent;
            idRect.width = fieldWidth * 0.75f + DrawingUtilities.indent;

            EditorGUI.PropertyField(idRect, property, GUIContent.none);
            GUI.color = baseGuiColor;
            
            var buttonRect = idRect;
            buttonRect.x += idRect.width + EditorGUIUtility.standardVerticalSpacing;
            buttonRect.width = EditorGUIUtility.singleLineHeight;

            if (!match)
            {
                GUI.enabled = false;
                isExpanded = false;

                var content = EditorGUIUtility.IconContent("animationvisibilitytoggleoff@2x");
                GUI.Button(buttonRect, content, EditorStyles.label);
                
                GUI.enabled = true;
            }
            else
            {
                var content = isExpanded ? EditorGUIUtility.IconContent("animationvisibilitytoggleon@2x") : EditorGUIUtility.IconContent("animationvisibilitytoggleoff@2x");
                if (GUI.Button(buttonRect, content, EditorStyles.label)) isExpanded = !isExpanded;

                if (!isExpanded) return;
                
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
            
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!hasBeenInitialized) Initialize();
            if (!isExpanded) return EditorGUIUtility.singleLineHeight;
            
            property.NextVisible(true);
            var encounterId = property.stringValue;

            property.NextVisible(false);
            var id = property.stringValue;

            var match = BeauTambourUtilities.DialogueProvider.TryGetDialogue(encounterId, id, runtimeSettings.Language, out var dialogue);

            if (match)
            {
                var style = new GUIStyle(EditorStyles.textArea);
                var textContent = new GUIContent(dialogue.ToString());

                var textHeight = style.CalcHeight(textContent, EditorGUIUtility.currentViewWidth);
                if (textHeight > EditorGUIUtility.singleLineHeight * 3f) textHeight = EditorGUIUtility.singleLineHeight * 3f;
                
                return EditorGUIUtility.singleLineHeight  +  textHeight;
            }
            else return EditorGUIUtility.singleLineHeight;
        }

        private void Initialize()
        {
            runtimeSettings = AssetDatabase.LoadAssetAtPath<RuntimeSettings>("Assets/Settings/RuntimeSettings.asset");
            hasBeenInitialized = true;
        }
    }
}