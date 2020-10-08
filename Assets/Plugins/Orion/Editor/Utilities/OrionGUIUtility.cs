using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public static class OrionGUIUtility 
    {
        public static Texture GetSmallIcon(this Type type) => GetTypeIcon(type, 16);
        public static Texture GetIcon(this Type type) => GetTypeIcon(type, 32);
        private static Texture GetTypeIcon(Type type, int resolution)
        {
            if (type == typeof(Enum)) return Resources.Load<Texture>($"Icons/Types/enum@{resolution}x");
            
            if (type.Namespace != null && type.Namespace.Contains("Orion"))
            {
                var guids = AssetDatabase.FindAssets(type.Name, new string[] {"Assets/Plugins/Orion/Core"});
                if (guids.Any())
                {
                    var meta = File.ReadAllLines($"{AssetDatabase.GUIDToAssetPath(guids.First())}.meta");

                    if (meta[7] != "  icon: {instanceID: 0}")
                    {
                        var startIndex =  meta[7].IndexOf("guid: ", StringComparison.Ordinal) + 6;
                        var endIndex =  meta[7].IndexOf(',', startIndex);
                
                        var iconPath = AssetDatabase.GUIDToAssetPath(meta[7].Substring(startIndex, endIndex - startIndex));
                        return AssetDatabase.LoadAssetAtPath<Texture>(iconPath);
                    }
                }
            } 
            
            if (type == typeof(Object)) return Resources.Load<Texture>($"Icons/Types/object@{resolution}x");
            if (typeof(Object).IsAssignableFrom(type)) return EditorGUIUtility.ObjectContent(null, type).image;
            if (type.IsExtendedPrimitive()) return Resources.Load<Texture>($"Icons/Types/{type.GetNiceName()}@{resolution}x");
            
            var texture = Resources.Load<Texture>($"Icons/Types/{type.GetNiceFullName()}@{resolution}x");
            if (texture == null)
            {
                if (type.IsArray)
                {
                    var elementType = type.GetElementType();
                    texture = Resources.Load<Texture>($"Icons/Types/System.Collections.Generic.IEnumerable_{elementType.GetNiceName()}@{resolution}x");
 
                    if (texture == null) texture = Resources.Load<Texture>($"Icons/Types/System.Collections.IEnumerable@{resolution}x");
                        
                    return texture;
                }
                    
                var interfaces = type.GetInterfaces();
                foreach (var interfaceType in interfaces)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(interfaceType))
                    {
                        if (interfaceType.IsGenericType)
                        {
                            var argumentType = interfaceType.GetGenericArguments().First();
                            texture = Resources.Load<Texture>($"Icons/Types/System.Collections.Generic.IEnumerable_{argumentType.GetNiceName()}@{resolution}x");
                        }
                        else texture = Resources.Load<Texture>($"Icons/Types/System.Collections.IEnumerable@{resolution}x");

                        return texture;
                    }
                }

                if (type.IsClass) return Resources.Load<Texture>($"Icons/Language/VisualStudioColor/Class@{resolution}x");
                if (type.IsValueType) return Resources.Load<Texture>($"Icons/Language/VisualStudioColor/Struct@{resolution}x");
                if (type.IsInterface) return Resources.Load<Texture>($"Icons/Language/VisualStudioColor/Interface@{resolution}x");
            }

            return texture;
        }

        public static Type GetProxyType(this InspectorProperty property)
        {
            var type = default(Type);
            
            if (property.ValueEntry.TypeOfValue.IsGenericType) type = property.ValueEntry.TypeOfValue.GetGenericArguments().First();
            else type = property.BaseValueEntry.TypeOfValue.BaseType.GetGenericArguments().First();

            return type;
        }
    }
}