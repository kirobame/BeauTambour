using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    public static class BeauTambourUtilities
    {
        public static IReadOnlyDictionary<string, string> Outcomes => outcomes;
        private static Dictionary<string, string> outcomes = new Dictionary<string, string>();

        [InitializeOnLoadMethod]
        private static void RegisterOutcomes()
        {
            outcomes.Clear();
            var guids = AssetDatabase.FindAssets("t:Outcome");
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var outcome = AssetDatabase.LoadAssetAtPath<Outcome>(path);

                if (!TryGetPrefabName(outcome, out var name)) return;
                outcomes.Add(path, name);
            }
        }

        public static void ModifyPathFor(Outcome outcome)
        {
            var key = string.Empty;
            foreach (var keyValuePair in outcomes)
            {
                var asset = AssetDatabase.LoadAssetAtPath<Outcome>(keyValuePair.Key);
                if (!TryGetPrefabName(asset, out var itemName) || keyValuePair.Value != itemName) continue;

                key = keyValuePair.Key;
                break;
            }

            if (key == string.Empty || !TryGetPrefabName(outcome, out var name)) return;
            
            outcomes.Remove(key);
            outcomes.Add(AssetDatabase.GetAssetPath(outcome), name);
        }

        private static bool TryGetPrefabName(Outcome outcome, out string name)
        {
            var serializedObject = new SerializedObject(outcome);
            var prefabProperty = serializedObject.FindProperty("sequencerPrefab");

            if (prefabProperty.objectReferenceValue != null)
            {
                name = prefabProperty.objectReferenceValue.name;
                return true;
            }
            else
            {
                name = string.Empty;
                return false;
            }
        }
    }
}