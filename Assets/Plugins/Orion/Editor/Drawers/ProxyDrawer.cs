using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public class ProxyDrawer<T,TElement> : OdinValueDrawer<T>  where T : IReadable<TElement>, IWritable<TElement>, IProxy<TElement>
    {
        private static readonly Type[] collectionTypes = new Type[]
        {
            typeof(ICollection),
            typeof(IList)
        };
        private static readonly Type[] inlinedTypes = new Type[]
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4)
        };

        private bool canDraw = true;
        private Type previousType;
        
        private LocalPersistentContext<bool> foldout;
        private Texture icon;
        
        private Rect rect = Rect.zero;

        private Action<GUIContent,InspectorProperty> draw;

        protected override void Initialize()
        {
            base.Initialize();
            Setup();

            Property.Context.GetPersistent(this, $"{Property}-Proxy-Foldout",out foldout);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (!canDraw)
            {
                CallNextDrawer(label);
                return;
            }

            var type = Property.Children.First().ValueEntry.TypeOfValue;
            if (type != previousType) Setup();
            else previousType = type;
            
            var text = label.text.Trim();
            label = new GUIContent("  " + text, icon);
            var childProperty = Property.Children.FirstOrDefault();
            
            draw(label, childProperty);
        }

        private void Setup()
        {
            if (Property.BaseValueEntry.WeakSmartValue == null || !Property.BaseValueEntry.BaseValueType.IsInterface)
            {
                canDraw = false;
                return;
            }
            
            icon = typeof(TElement).GetIcon();
            var childProperty = Property.Children.First();

            var isCollection = collectionTypes.Any(type => type.IsAssignableFrom(typeof(TElement)));
            var isObject = typeof(Object).IsAssignableFrom(typeof(TElement));
            
            var count = Property.Children.Count;
            var subCount = childProperty.Children.Count;
            
            var isIndirectlyInlined = Property.Attributes.Contains(new InlinedAttribute());
            var isDirectlyInlined = inlinedTypes.Any(type => type.IsAssignableFrom(typeof(TElement)));

            if (isDirectlyInlined) draw = DrawDirectlyInlined;
            else if (isObject || isIndirectlyInlined)    draw = DrawIndirectlyInlined;
            else if (count > 1) draw = DrawAsIndirectFoldout;
            else if (isCollection) draw = DrawAsCollection;
            else if (subCount <= 1)draw = DrawDirectlyInlined;
            else draw = DrawAsDirectFoldout;

            previousType = childProperty.ValueEntry.TypeOfValue;
        }
        
        #region Drawing Methods
        
        private void DrawDirectlyInlined(GUIContent label, InspectorProperty property)
        {
            var size = EditorGUIUtility.singleLineHeight;
            if (property.ValueEntry.TypeOfValue.GetCustomAttribute<InlinePropertyAttribute>() == null)SubDraw();
            else
            {
                EditorGUILayout.Space(-2f);
                GUIHelper.PushLabelWidth(GUIHelper.BetterLabelWidth - GUIHelper.CurrentIndentAmount + 3f);
                    
                SubDraw();
                    
                GUIHelper.PopLabelWidth();
                EditorGUILayout.Space(5f);
            }

            void SubDraw()
            {
                EditorGUILayout.BeginHorizontal();
                property.Draw(label);

                if (GUILayout.Button(EditorIcons.X.Active, GUILayout.Width(size), GUILayout.Height(size)))
                {
                    Property.BaseValueEntry.WeakSmartValue = null;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        private void DrawIndirectlyInlined(GUIContent label, InspectorProperty property)
        {
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(label, GUILayout.Width(GUIHelper.BetterLabelWidth - GUIHelper.CurrentIndentAmount));
            for (var i = 0; i < Property.Children.Count; i++) Property.Children[i].Draw(GUIContent.none);
            
            var size = EditorGUIUtility.singleLineHeight;
            if (GUILayout.Button(EditorIcons.X.Active, GUILayout.Width(size), GUILayout.Height(size)))
            {
                Property.BaseValueEntry.WeakSmartValue = null;
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawAsCollection(GUIContent label, InspectorProperty property)
        {
            if (property.ValueEntry.WeakSmartValue == null)
            {
                var firstChildType = property.ValueEntry.TypeOfValue;
                property.ValueEntry.WeakSmartValue = Activator.CreateInstance(firstChildType,0);
            }
                
            EditorGUILayout.Space(1f);
            EditorGUILayout.BeginHorizontal();

            var size = EditorGUIUtility.singleLineHeight;
            EditorGUILayout.LabelField(new GUIContent(icon), GUILayout.Width(size + 3f), GUILayout.Height(size));
                
            Property.Children.First().Draw(new GUIContent(label.text));

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(-1f);
        }

        private void DrawAsIndirectFoldout(GUIContent label, InspectorProperty property)
        {
            foldout.Value = SirenixEditorGUI.Foldout(foldout.Value, label);
            if (SirenixEditorGUI.BeginFadeGroup(this, foldout.Value))
            {
                GUIHelper.PushIndentLevel(1);
                for (var i = 0; i < Property.Children.Count; i++) Property.Children[i].Draw();
                GUIHelper.PopIndentLevel();
            }
            SirenixEditorGUI.EndFadeGroup();
        }

        private void DrawAsDirectFoldout(GUIContent label, InspectorProperty property)
        {
            var size = EditorGUIUtility.singleLineHeight;
            if (rect != Rect.zero)
            {
                rect = new Rect(new Vector2(rect.x + GUIHelper.BetterLabelWidth - GUIHelper.CurrentIndentAmount + 4f, rect.y), Vector2.one * size);
                if (GUI.Button(rect, EditorIcons.X.Active)) Property.BaseValueEntry.WeakSmartValue = null;
            }
            if (!Property.Children.Any()) return;
                
            GUIHelper.PushLabelWidth(GUIHelper.BetterLabelWidth + 3f);
                
            Property.Children.First().Draw(label);
            rect = Property.LastDrawnValueRect;
                
            GUIHelper.PopLabelWidth();
        }
        #endregion
    }
}