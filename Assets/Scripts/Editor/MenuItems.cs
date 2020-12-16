using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Flux;
using Flux.Editor;
using UnityEditor;
using UnityEngine;

namespace BeauTambour.Editor
{
    public static class MenuItems
    {
        [MenuItem("Assets/Create/Beau Tambour/Outcome", priority = -999999999)]
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
            BeauTambourUtilities.Bootup();
            
            var folders = new HashSet<string>();
            var outcomes = new List<Outcome>();
            
            var guids = AssetDatabase.FindAssets("t:Outcome");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var split = path.Split('/');
                
                var folder = string.Empty;
                for (var i = 0; i < split.Length - 2; i++) folder += $"{split[i]}/";
                folder = folder.Remove(folder.Length - 1);
                
                folders.Add(folder);
                outcomes.Add(AssetDatabase.LoadAssetAtPath<Outcome>(path));
            }
            
            var sequencerSource = Resources.Load<GameObject>("Prefabs/DialogueSequencer");
            foreach (var keyValuePair in BeauTambourUtilities.DialogueProvider.Sheets)
            {
                var sheet = keyValuePair.Value;
                
                var rowKeys = sheet.RowKeys["Dialogues"];
                foreach (var rowKey in rowKeys)
                {
                    var name = $"{sheet.Source.Name}-{rowKey}";

                    var exists = outcomes.Exists(outcome => outcome.name == name);
                    if (exists) continue;

                    var reference = new DialogueReference(keyValuePair.Key, rowKey);
                    var folder = folders.FirstOrDefault(path => path.Split('/').Last() == sheet.Source.Name);

                    if (folder != null)
                    {
                        var path = $"{folder}/Outcomes";
                        if (!AssetDatabase.IsValidFolder(path)) AssetDatabase.CreateFolder(folder, "Outcomes");
                        
                        InstantiateDialogueOutcome(sheet, reference, path, name, sequencerSource);
                    }
                    else
                    {
                        var path = "Assets/Objects/Narration/Encounters";
                        Debug.Log(sheet.Source.Name);
                        AssetDatabase.CreateFolder(path, sheet.Source.Name);
                        
                        path += $"/{sheet.Source.Name}";
                        folders.Add(path);
                        AssetDatabase.CreateFolder(path, "Outcomes");

                        path += "/Outcomes";
                        InstantiateDialogueOutcome(sheet, reference, path, name, sequencerSource);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static (Outcome outcome, Sequencer sequencer) InstantiateOutcome(string path, string name, GameObject sequencerSource)
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
            
            return (outcome, sequencer.GetComponent<Sequencer>());
        }
        private static void InstantiateDialogueOutcome(RuntimeSheet sheet, DialogueReference reference, string path, string name, GameObject sequencerSource)
        {
            var result = InstantiateOutcome(path, name, sequencerSource);
            var effect = result.sequencer.GetComponent<DialogueEffect>();
            effect.reference = reference;

            AddPhaseCondition(reference, result.outcome);
            
            var rowKey = name.Split('-')[1];
            TryFor("Priority", typeof(PriorityInterpreter));
            TryFor("Emotion", typeof(IsEmotionMetInterpreter));
            TryFor("Keep", typeof(RemovalInterpreter));
            TryFor("Needed Musician", typeof(NeedsMusicianInterpreter));
            
            void TryFor(string key, Type type)
            {
                var data = sheet["Dialogues", key, rowKey];
                if (data == string.Empty || data == "None") return;
                
                BeauTambourUtilities.OutcomeInterpreters[type].Interpret(data, result.outcome, result.sequencer);
            }
            
            var header = sheet["Dialogues", "Data", rowKey];
            if (header == string.Empty) return;
            
            foreach (var interpreter in BeauTambourUtilities.OutcomeInterpreters.Values)
            {
                interpreter.TryFor(header, result.outcome, result.sequencer);
            }
        }

        private static void AddPhaseCondition(DialogueReference dialogueReference, Outcome outcome)
        {
            var condition = ScriptableObject.CreateInstance<PhaseCondition>();
            condition.hideFlags = HideFlags.HideInHierarchy;
            condition.name = $"{Guid.NewGuid()}-{typeof(PhaseCondition).Name}";
            
            AssetDatabase.AddObjectToAsset(condition, outcome);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(condition));
            
            var conditionSerializedObject = new SerializedObject(condition);
            var targetProperty = conditionSerializedObject.FindProperty("joinedIds");

            targetProperty.stringValue = dialogueReference.EncounterId;
            conditionSerializedObject.ApplyModifiedProperties();
            
            var serializedObject = new SerializedObject(outcome);
            var conditionsProperty = serializedObject.FindProperty("conditions");
            
            conditionsProperty.AddElement(condition);
            serializedObject.ApplyModifiedProperties();
        }
    }
}