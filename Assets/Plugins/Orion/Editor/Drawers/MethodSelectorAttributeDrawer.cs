using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public class MethodSelectorAttributeDrawer : OdinAttributeDrawer<MethodSelectorAttribute>
    {
        private GenericSelector<MethodInfo> dropdown;
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
            
            var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            bool IsMethodValid(MethodInfo info)
            {
                return Attribute.returnType.IsAssignableFrom(info.ReturnType) && info.IsGenericMethod == Attribute.allowGenerics;
            }
            string GetItemName(MethodInfo info)
            {
                var name = info.Name.Remove(0, 4);
                
                if (info.Name.Contains("set_")) return $"Properties/{name}({info.GetParameters().First().ParameterType.Name} value)";
                else if (info.Name.Contains("get_")) return $"Properties/{info.ReturnType.Name} {name}()";
                else
                {
                    if (methods.Count(method => method.Name == info.Name) > 1) return $"Methods/{info.Name}/{info.GetNiceName()}";
                    else return $"Methods/{info.GetNiceName()}";
                }
            }
            var items = methods.Where(IsMethodValid).ToArray();
            
            dropdown = new GenericSelector<MethodInfo>(string.Empty, false, GetItemName, items);
            dropdown.SelectionTree.DefaultMenuStyle.Height = (int) EditorGUIUtility.singleLineHeight + 4;

            dropdown.SelectionChanged += SaveProperty;
            dropdown.SelectionConfirmed += SaveProperty;
        }

        private void SaveProperty(IEnumerable<MethodInfo> methods)
        {
            var method = methods.FirstOrDefault();
            if (method == null) return;
            
            var parameters = method.GetParameters();
            
            var name = method.Name;
            for (var i = 0; i < parameters.Length; i++) name += $"/{parameters[i].ParameterType.AssemblyQualifiedName}";

            Property.ValueEntry.WeakSmartValue = name;
        }
    }
}