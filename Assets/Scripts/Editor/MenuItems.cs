using System;
using Ludiq.PeekCore;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    public static class MenuItems
    {
        [MenuItem("Assets/Create/Beau Tambour/Resolution/Outcome", priority = -999999999)]
        public static void CreateOutcome()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == string.Empty) path = "Assets";
            
            var sequencerSource = Resources.Load<GameObject>("Prefabs/Sequencer");

            var guid = Guid.NewGuid().ToString();
            var sequencerPath = $"Assets/Prefabs/Outcomes/{guid}.prefab";
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(sequencerSource), sequencerPath);

            var outcome = ScriptableObject.CreateInstance<Outcome>();
            AssetDatabase.CreateAsset(outcome, $"{path}/NewOutcome.asset");

            var sequencer = AssetDatabase.LoadAssetAtPath<GameObject>(sequencerPath);
            var serializedObject = new SerializedObject(outcome);
            
            var prefabProperty = serializedObject.FindProperty("sequencerPrefab");
            prefabProperty.objectReferenceValue = sequencer;

            serializedObject.ApplyModifiedProperties();
            Selection.activeObject = outcome;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}