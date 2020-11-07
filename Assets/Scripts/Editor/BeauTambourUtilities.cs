using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    public static class BeauTambourUtilities
    {
        public static DialogueProvider DialogueProvider { get; private set; }
        
        public static IReadOnlyDictionary<string, string> Outcomes => outcomes;
        
        private static Dictionary<string, string> outcomes = new Dictionary<string, string>();
        private static Dictionary<Outcome, string> pathHistoric = new Dictionary<Outcome, string>();

        [InitializeOnLoadMethod]
        private static void Bootup()
        {
            RegisterOutcomes();

            var gameObject = EditorUtility.CreateGameObjectWithHideFlags("Editor-DialogueProvider", HideFlags.HideAndDontSave, typeof(DialogueProvider));
            DialogueProvider = gameObject.GetComponent<DialogueProvider>();

            var guids = AssetDatabase.FindAssets("Dialogues t:CSVRecipient");
            var dialogues = AssetDatabase.LoadAssetAtPath<CSVRecipient>(AssetDatabase.GUIDToAssetPath(guids.First()));
            
            DialogueProvider.Process(dialogues.Sheets.ToArray());
        }
        private static void RegisterOutcomes()
        {
            outcomes.Clear();
            pathHistoric.Clear();
            
            var guids = AssetDatabase.FindAssets("t:Outcome");
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var outcome = AssetDatabase.LoadAssetAtPath<Outcome>(path);

                if (!TryGetPrefabName(outcome, out var name)) return;
                outcomes.Add(path, name);
                pathHistoric.Add(outcome, path);
            }
        }

        public static void ModifyPathFor(Outcome outcome)
        {
            if (pathHistoric.TryGetValue(outcome, out var oldPath))
            {
                var newPath = AssetDatabase.GetAssetPath(outcome);
                if (outcomes.ContainsKey(newPath)) return;
                
                var prefabName = outcomes[oldPath];
                
                outcomes.Remove(oldPath);
                outcomes.Add(newPath, prefabName);
                
                pathHistoric[outcome] = newPath;
            }
            else if (TryGetPrefabName(outcome, out var prefabName))
            {
                var path = AssetDatabase.GetAssetPath(outcome);
                
                outcomes.Add(path, prefabName);
                pathHistoric.Add(outcome, path);
            }
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