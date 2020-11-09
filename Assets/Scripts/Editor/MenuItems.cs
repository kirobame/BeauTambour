using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Flux;
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

            var result = InstantiateOutcome(path, "NewOutcome",Resources.Load<GameObject>("Prefabs/Sequencer"));
            Selection.activeObject = result.outcome;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Beau Tambour/Update Outcomes")]
        public static void UpdateOutcomes()
        {
            var folders = new HashSet<string>();
            var outcomes = new List<Outcome>();
            
            var guids = AssetDatabase.FindAssets("t:Outcome");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                
                folders.Add(Directory.GetParent(Path.GetDirectoryName(path)).ToString());
                outcomes.Add(AssetDatabase.LoadAssetAtPath<Outcome>(path));
            }

            var sequencerSource = Resources.Load<GameObject>("Prefabs/DialogueSequencer");
            
            for (var encounterIndex = 0; encounterIndex < BeauTambourUtilities.DialogueProvider.Sheets.Count; encounterIndex++)
            {
                var sheet = BeauTambourUtilities.DialogueProvider.Sheets[encounterIndex];
                
                var rowKeys = sheet.RowKeys["Dialogues"];
                foreach (var rowKey in rowKeys)
                {
                    var name = $"{sheet.Source.Name}-{rowKey}";
                    var dialogue = sheet["Dialogues", SupportedLanguage.Français.ToString(), rowKey];

                    var exists = outcomes.Exists(outcome => outcome.name == name);
                    if (exists) continue;

                    var reference = new DialogueReference(encounterIndex + 1, rowKey);

                    var folder = folders.FirstOrDefault(path => path.Split('/').Last() == sheet.Source.Name);
                    if (folder != null)
                    {
                        InstantiateDialogueOutcome(reference, $"{folder}/Outcomes", name, sequencerSource);
                    }
                    else
                    {
                        var path = $"Assets/Objects/Narration/Encounters/{sheet.Source.Name}/Outcomes";
                        InstantiateDialogueOutcome(reference, path, name, sequencerSource);
                    }
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static (Outcome outcome, GameObject sequencer) InstantiateOutcome(string path, string name, GameObject sequencerSource)
        {
            var guid = Guid.NewGuid().ToString();
            var sequencerPath = $"Assets/Prefabs/Outcomes/{guid}.prefab";
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(sequencerSource), sequencerPath);

            var outcome = ScriptableObject.CreateInstance<Outcome>();
            AssetDatabase.CreateAsset(outcome, $"{path}/{name}.asset");

            var sequencer = AssetDatabase.LoadAssetAtPath<GameObject>(sequencerPath);
            var serializedObject = new SerializedObject(outcome);
            
            var prefabProperty = serializedObject.FindProperty("sequencerPrefab");
            prefabProperty.objectReferenceValue = sequencer;

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(sequencer);
            
            return (outcome, sequencer);
        }
        private static void InstantiateDialogueOutcome(DialogueReference reference, string path, string name, GameObject sequencerSource)
        {
            var result = InstantiateOutcome(path, name, sequencerSource);
            var effect = result.sequencer.GetComponent<DialogueEffect>();

            effect.reference = reference;
        }
    }
}