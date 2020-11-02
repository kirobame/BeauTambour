using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Flux.Editor
{
    public abstract class EmbeddedScriptableCollectionDrawer<TScriptable> : EmbeddedCollectionDrawer<TScriptable> where TScriptable : ScriptableObject
    {
        public EmbeddedScriptableCollectionDrawer(SerializedObject serializedObject, SerializedProperty property) : base(serializedObject, property) { }

        protected override TScriptable CreateItem(Type itemType)
        {
            var root = reorderableList.serializedProperty.serializedObject.targetObject as ScriptableObject;
            
            var condition = (TScriptable)ScriptableObject.CreateInstance(itemType);
            condition.hideFlags = HideFlags.HideInHierarchy;
            condition.name = $"{Guid.NewGuid()}-{itemType.Name}";
            
            AssetDatabase.AddObjectToAsset(condition, root);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(condition));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            return condition;
        }
        protected override void DeleteItem(TScriptable item)
        {
            Object.DestroyImmediate(item, true);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}