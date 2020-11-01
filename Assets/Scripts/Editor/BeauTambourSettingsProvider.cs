using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BeauTambour.Editor
{
    class BeauTambourSettingsProvider : SettingsProvider
    {
        #region MyRegion

        class Keywords
        {
            public static GUIContent rythmMarginTolerance = new GUIContent("Rythm Margin Tolerance");
            public static GUIContent toleranceRadiusRange = new GUIContent("Tolerance Radius Range");
            public static GUIContent standardToleranceRadius = new GUIContent("Standard Tolerance Radius");
            public static GUIContent curveSubdivision = new GUIContent("Curve Subdivision");
            public static GUIContent recognitionErrorThreshold = new GUIContent("Recognition Error Threshold");
            public static GUIContent headingErrorFactor = new GUIContent("Heading Error Factor");
            public static GUIContent SpacingErrorFactor = new GUIContent("Spacing Error Factor");
        }
        #endregion
        
        public BeauTambourSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }
        
        private SerializedObject instance;
        
        public static bool IsSettingsAvailable() => File.Exists(BeauTambourSettings.path);
        public override void OnActivate(string searchContext, VisualElement rootElement) => instance = BeauTambourSettings.GetSerializedSettings();

        public override void OnGUI(string searchContext)
        {
            instance.UpdateIfRequiredOrScript();
            
            var originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth += 100f;
            
            var iterator = instance.GetIterator();
            iterator.NextVisible(true);

            DrawTitle("Rythm Operation");
            
            iterator.NextVisible(false);
            DrawElement(() => EditorGUILayout.Slider(iterator, 0.01f, 2f));
            
            EditorGUILayout.Space();
            DrawTitle("Shape Recognition");
            
            iterator.NextVisible(false);
            var radiusToleranceRange = iterator.vector2Value;
            DrawElement(() => EditorGUILayout.PropertyField(iterator));
            
            iterator.NextVisible(false);
            iterator.floatValue = Mathf.Clamp(iterator.floatValue, radiusToleranceRange.x, radiusToleranceRange.y);
            DrawElement(() => EditorGUILayout.Slider(iterator, radiusToleranceRange.x, radiusToleranceRange.y));
            iterator.NextVisible(false);
            DrawElement(() => EditorGUILayout.Slider(iterator, 0f, 3f));
            
            iterator.NextVisible(false);
            DrawElement(() => EditorGUILayout.IntSlider(iterator, 1, 6));

            iterator.NextVisible(false);
            DrawElement(() => EditorGUILayout.Slider(iterator, 0.5f, 15f));
            iterator.NextVisible(false);
            DrawElement(() => EditorGUILayout.Slider(iterator, 0.01f, 1f));
            iterator.NextVisible(false);
            DrawElement(() => EditorGUILayout.Slider(iterator, 0.01f, 1f));

            EditorGUIUtility.labelWidth = originalLabelWidth;
            instance.ApplyModifiedProperties();
        }

        private void DrawTitle(string title)
        {
            var titleStyle = new GUIStyle(EditorStyles.largeLabel);
            titleStyle.fontStyle = FontStyle.Bold;

            var label = new GUIContent(title);
            var option = GUILayout.Height(EditorGUIUtility.singleLineHeight + 5f);
            DrawElement(() => EditorGUILayout.LabelField(label, titleStyle, option), 6f);
            
            DrawSeparation();
            EditorGUILayout.Space();
        }
        private void DrawElement(Action draw, float spacing = 7f)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(spacing, false);
            
            draw();
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(EditorGUIUtility.standardVerticalSpacing * 2f);
        }
        private void DrawSeparation()
        {
            var rect = GUILayoutUtility.GetLastRect();
            rect.x += 12f;
            rect.width -= 12f;
            rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
            rect.height = 1f;
            
            EditorGUI.DrawRect(rect, new Color(0.1411765f,0.1411765f,0.1411765f));
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateBeauTambourSettingsProvider()
        {
            if (!IsSettingsAvailable()) BeauTambourSettings.CreateSettings();
            {
                var provider = new BeauTambourSettingsProvider("Project/Beau Tambour", SettingsScope.Project);
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<Keywords>();
                
                return provider;
            }
            
            return null;
        }
    }
}