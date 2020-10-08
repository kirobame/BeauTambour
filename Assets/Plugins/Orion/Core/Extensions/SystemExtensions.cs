using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orion
{
    public static class SystemExtensions 
    {
        public static IEnumerable<Type> GetDependencies(this Type root)
        {
            var dependencies = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!root.IsAssignableFrom(type) || type.IsAbstract) continue;
                    dependencies.Add(type);
                }
            }

            return dependencies;
        }
        
        public static bool IsExtendedPrimitive(this Type type)
        {
            if (type.IsPrimitive) return true;
            else if (type == typeof(string)) return true;
            else if (type == typeof(object)) return true;
            else if (type.BaseType == typeof(Enum)) return true;
            
            return false;
        }
    }
}