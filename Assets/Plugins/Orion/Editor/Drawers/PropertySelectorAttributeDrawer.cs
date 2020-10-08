using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public class PropertySelectorAttributeDrawer : OdinAttributeDrawer<PropertySelectorAttribute>
    {
        private GenericSelector<PropertyInfo> dropdown;
        private Object previousTarget;

        protected override void Initialize()
        {
            base.Initialize();
            if (!TryGetTarget(out var target)) return;

            OnTargetChanged(target);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (Property.ValueEntry.TypeOfValue != typeof(string))
            {
                DrawAsDefault(label);
                return;
            }
            
            if (!TryGetTarget(out var target))
            {
                DrawAsDefault(label);
                return;
            }
            
            if (target != previousTarget) OnTargetChanged(target);
            previousTarget = target;

            var name = ((string)Property.ValueEntry.WeakSmartValue).Split('/').First();
            if (GUILayout.Button(new GUIContent(name), EditorStyles.popup))
            {
                if (target == null) return;
                
                var window = dropdown.ShowInPopup();
                window.OnClose += dropdown.SelectionTree.Selection.ConfirmSelection; 
            }
        }

        private bool TryGetTarget(out Object target)
        {
            var targetProperty = Property.Parent.FindChild(child => child.Name == Attribute.targetName, false);
            if (targetProperty == null)
            {
                target = null;
                return false;
            }
            
            target = targetProperty.ValueEntry.WeakSmartValue as Object;
            return true;
        }

        private void DrawAsDefault(GUIContent label) => CallNextDrawer(label);
        
        private void OnTargetChanged(Object target)
        {
            if (target == null)
            {
                Property.ValueEntry.WeakSmartValue = "None";
                return;
            }
            
            var methods = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            
            var wantedType = default(Type);
            if (Attribute.genericTypeIndex != -1) wantedType = Property.Parent.ValueEntry.TypeOfValue.GetGenericArguments()[Attribute.genericTypeIndex];
            else wantedType = Attribute.type;
            
            bool IsPropertyValid(PropertyInfo info)
            {
                if (!info.CanRead || !info.CanWrite) return false;
                return wantedType.IsAssignableFrom(info.PropertyType);
            }
            string GetItemName(PropertyInfo info) => $"{info.PropertyType.Name} {info.Name}";
            
            var items = methods.Where(IsPropertyValid).ToArray();
            
            dropdown = new GenericSelector<PropertyInfo>(string.Empty, false, GetItemName, items);
            dropdown.SelectionTree.DefaultMenuStyle.Height = (int) EditorGUIUtility.singleLineHeight + 4;

            dropdown.SelectionChanged += SaveProperty;
            dropdown.SelectionConfirmed += SaveProperty;
        }

        private void SaveProperty(IEnumerable<PropertyInfo> properties)
        {
            var property = properties.FirstOrDefault();
            if (property == null) return;
            
            Property.ValueEntry.WeakSmartValue = property.Name;
        }
    }
}