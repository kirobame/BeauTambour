using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Deprecated;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    [IconIndicator(7705900795600745325), CreateAssetMenu(fileName = "NewEncounter", menuName = "Beau Tambour/Encounter")]
    public class Encounter : ScriptableObject
    {
        [SerializeField] private CSVRecipient dialogueRecipient;
        [SerializeField] private int sheetIndex;
        
        [SerializeField] private Musician[] initialMusicians;
        
        public void Bootup(MonoBehaviour hook, bool useBackup)
        {
            Repository.Reference(this, References.Encounter);
            Event.Open(GameEvents.OnEncounterBootedUp);

            if (!useBackup) hook.StartCoroutine(dialogueRecipient.Download(OnDialogueSheetsDownloaded));
            else OnDialogueSheetsDownloaded(dialogueRecipient.Sheets.ToArray());
        }

        private void OnDialogueSheetsDownloaded(Sheet[] sheets)
        {
            var runtimeSheet = new RuntimeSheet();
            runtimeSheet.Process(sheets[sheetIndex]);

            var rows = runtimeSheet.RowKeys["Dialogues"];
            foreach (var row in rows)
            {
                var type = runtimeSheet["Dialogues", "Type", row];
                
                var languages = Enum.GetNames(typeof(Language));
                var texts = new string[languages.Length];
                for (var i = 0; i < languages.Length; i++) texts[i] = runtimeSheet["Dialogues", languages[i], row];

                var rawData = runtimeSheet["Dialogues", "Data", row];
                rawData = Regex.Replace(rawData, "\r\n|\r|\n", string.Empty);
                rawData = rawData.Replace(" ", string.Empty);

                var data = new Dictionary<string, string>();
                var splittedData = rawData.Split(new char[] {':', ';'}, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < splittedData.Length - 1; i += 2) data.Add(splittedData[i], splittedData[i + 1]);

                switch (type)
                {
                    case "Hint":
                        if (TryGetCharacter<Musician>(runtimeSheet["Dialogues", "Source", row], out var hintMusician)) HandleHint(row, texts, data, hintMusician);
                        break;
                    
                    case "Advance":
                        if (TryGetCharacter<Musician>(runtimeSheet["Dialogues", "Source", row], out var advanceMusician)) HandleAdvance(row, texts, data, advanceMusician);
                        break;
                    
                    case "Event":
                        HandleEventBoundDialogue(texts, data);
                        break;
                    
                    case "Harmony":
                        if (TryGetCharacter<Interlocutor>(runtimeSheet["Dialogues", "Source", row], out var interlocutor))  HandleHarmonyDialogue(texts, data, interlocutor);
                        break;
                }
            }
            
            Event.Call(GameEvents.OnEncounterBootedUp);
            GameState.PassBlock();

            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            phaseHandler.Play(PhaseCategory.Dialogue);
        }

        private bool TryGetCharacter<TChar>(string source, out TChar musician) where TChar : Character
        {
            musician = null;

            if (Enum.TryParse<Actor>(source, out var actor))
            {
                musician = Extensions.GetCharacter<TChar>(actor);
                return true;
            }
            else return false;
        }
        
        private void HandleHint(string row, string[] texts, Dictionary<string, string> data, Musician musician)
        {
            if (!data.ContainsKey("Name")) return;
            musician.AddDialogueFailsafe(data["Name"], texts);
        }
        private void HandleAdvance(string row, string[] texts, Dictionary<string, string> data, Musician musician)
        {
            var dialogueNode = new Musician.DialogueNode(row);
            if (!dialogueNode.TryProcess(texts, data)) return;
            
            if (data.ContainsKey("Root")) musician.AddDialogueNodeRootKey(row, int.Parse(data["Root"]) - 1);
            musician.AddDialogueNode(dialogueNode);
        }
        
        private void HandleHarmonyDialogue(string[] texts, Dictionary<string, string> data, Interlocutor interlocutor)
        {
            if (!data.ContainsKey("Emotion") || Enum.TryParse<Emotion>(data["Emotion"], out var emotion)) return;
            
            if (!data.ContainsKey("Block")) return;
            var block = int.Parse(data["Block"]);
            
            if (!data.ContainsKey("IsCorrect")) return;
            
            if (bool.Parse(data["IsCorrect"]) == true) interlocutor.AddBlockDialogue(emotion, texts, block);
            else interlocutor.AddDialogueOption(emotion, texts, block);
        }
        
        private void HandleEventBoundDialogue(string[] texts, Dictionary<string, string> data)
        {
            if (!data.ContainsKey("Key")) return;
            
            var eventBoundDialogue = new EventBoundDialogue(data["Key"], texts);
            GameState.AddEventBoundDialogue(eventBoundDialogue);
        }
    }
}