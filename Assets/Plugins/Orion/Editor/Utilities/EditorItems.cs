using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Orion.Editor
{
    public static class EditorItems
    {
        [MenuItem("GameObject/Reference" , false, -3000)]
        public static void ReferenceGameObject(MenuCommand command)
        {
            if (command.context is GameObject gameObject)
            {
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    var prefab = prefabStage.prefabContentsRoot;
                    Reference(prefab.transform, gameObject, gameObject, false, prefab);
                }
                else
                {
                    var rootFlag = gameObject.GetComponentInParent<Root>();
                    var root = rootFlag == null ? gameObject.transform.root : rootFlag.transform;

                    if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                    {
                        var prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
                        Reference(root, gameObject, gameObject, false, prefab);
                    }
                    else Reference(root, gameObject, gameObject, false, null);
                }
            }
        }
        [MenuItem("GameObject/StackReference" , false, -3000)]
        public static void StackReferenceGameObject(MenuCommand command)
        {
            if (command.context is GameObject gameObject)
            {
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    var prefab = prefabStage.prefabContentsRoot;
                    Reference(prefab.transform, gameObject, gameObject, true, prefab);
                }
                else
                {
                    var rootFlag = gameObject.GetComponentInParent<Root>();
                    var root = rootFlag == null ? gameObject.transform.root : rootFlag.transform;

                    if (PrefabUtility.IsPartOfAnyPrefab(gameObject))
                    {
                        var prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
                        Reference(root, gameObject, gameObject, true, prefab);
                    }
                    else Reference(root, gameObject, gameObject, true, null);
                }
            }
        }
        
        [MenuItem("CONTEXT/Object/Reference" , false, -3000)]
        public static void Reference(MenuCommand command)
        {
            if (command.context is Component component)
            {
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    var prefab = prefabStage.prefabContentsRoot;
                    Reference(prefab.transform, component.gameObject, component, false, prefab);
                }
                else
                {
                    var rootFlag = component.GetComponentInParent<Root>();
                    var root = rootFlag == null ? component.transform.root : rootFlag.transform;

                    if (PrefabUtility.IsPartOfAnyPrefab(component.gameObject))
                    {
                        var prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(component.gameObject);
                        Reference(root, component.gameObject, component, false, prefab);
                    }
                    else Reference(root, component.gameObject, component, false, null);
                }
            }
        }
        [MenuItem("CONTEXT/Object/StackReference" , false, -3000)]
        public static void StackReference(MenuCommand command)
        {
            if (command.context is Component component)
            {
                var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    var prefab = prefabStage.prefabContentsRoot;
                    Reference(prefab.transform, component.gameObject, component, true, prefab);
                }
                else
                {
                    var rootFlag = component.GetComponentInParent<Root>();
                    var root = rootFlag == null ? component.transform.root : rootFlag.transform;

                    if (PrefabUtility.IsPartOfAnyPrefab(component.gameObject))
                    {
                        var prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(component.gameObject);
                        Reference(root, component.gameObject, component, true, prefab);
                    }
                    else Reference(root, component.gameObject, component, true, null);
                }
            }
        }
        
        private static void Reference(Transform root, GameObject gameObject, Object value, bool stackReferencer, GameObject prefab)
        {
            var holder = default(GameObject);
            
            var existingReferencer = root.GetComponentInChildren<IReferencer>() as Component;
            if (existingReferencer != null) holder = existingReferencer.gameObject;
            else
            {
                holder = new GameObject("Referencer");
            
                GameObjectUtility.SetParentAndAlign(holder, root.gameObject);
                holder.transform.SetAsFirstSibling();
                
                Undo.RegisterCreatedObjectUndo(holder, "Create " + holder.name);
            }

            Object referencer = stackReferencer ? (Object)holder.AddComponent<StackReferencer>() : holder.AddComponent<Referencer>();

            var serializedObject = new SerializedObject(referencer);
            var valueProperty = serializedObject.FindProperty("value");
            valueProperty.objectReferenceInstanceIDValue = value.GetInstanceID();
            
            var rootPath = "Assets/Objects";
            if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);

            var componentIndex = Array.IndexOf(gameObject.GetComponents<Component>(), value);
            var type = componentIndex == -1 ? value.GetType().Name : $"[{componentIndex}]{value.GetType().Name}";
            
            var tokenPath = string.Empty;
            var isPrefab = false;
            if (prefab != null)
            {
                var prefabPath = $"{rootPath}/Prefabs";
                if (!Directory.Exists(prefabPath)) Directory.CreateDirectory(prefabPath);

                tokenPath = $"{prefabPath}/{prefab.name}-{type}-{value.name}Token.asset";
                isPrefab = true;
            }
            else
            {
                var scenePath = $"{rootPath}/Scenes";
                if (!Directory.Exists(scenePath)) Directory.CreateDirectory(scenePath);

                var activeScene = SceneManager.GetActiveScene();
                var currentScenePath = $"{scenePath}/{activeScene.name}";
                if (!Directory.Exists(currentScenePath)) Directory.CreateDirectory(currentScenePath);

                var number = activeScene.buildIndex.ToString().Remove(0, 1);
                var index = activeScene.buildIndex < 10 ? $"0{number}" : number;
                var prefix = string.Concat(activeScene.name.Where(c => c >= 'A' && c <= 'Z')) + index;
                tokenPath = $"{currentScenePath}/{prefix}-{type}-{value.name}Token.asset";
            }
            
            var token = ScriptableObject.CreateInstance<Token>();
            AssetDatabase.CreateAsset(token, tokenPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            var tokenProperty = serializedObject.FindProperty("token");
            tokenProperty.objectReferenceInstanceIDValue = token.GetInstanceID();
            
            serializedObject.ApplyModifiedProperties();
            if (isPrefab && PrefabStageUtility.GetCurrentPrefabStage() == null) PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.UserAction);
            
            Selection.activeObject = referencer;
            EditorGUIUtility.PingObject(referencer);
        }
    }
}