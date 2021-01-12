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
    [IconIndicator(7705900795600745325), CreateAssetMenu(fileName = "NewEncounter", menuName = "Beau Tambour/Chapter/Encounter")]
    public class Encounter : ScriptableObject
    {
        public Interlocutor Interlocutor { get; private set; }
        public int BlockCount => blocks.Length;
        
        [SerializeField] private CSVRecipient dialogueRecipient;
        [SerializeField] private int sheetIndex;

        [Space, SerializeField] private Block[] blocks;
        private Block previousBlock;

        private bool awaitingCurtain;
        private bool hasBeenBootedUp;
        
        private MonoBehaviour hook;

        public void Bootup(MonoBehaviour hook, bool useBackup)
        {
            this.hook = hook;
            hasBeenBootedUp = false;
            
            Repository.Reference(this, References.Encounter);
            
            Event.Open(GameEvents.OnEncounterBootedUp);
            Event.Open(GameEvents.OnCurtainFall);
            Event.Open(GameEvents.OnGoingToNextBlock);
            Event.Open(GameEvents.OnCurtainRaised);
            
            Event.Register(GameEvents.OnBlockPassed, OnBlockPassed);
            Event.Register<Dialogue>(GameEvents.OnDialogueFinished, OnDialogueFinished);
            
            if (!useBackup) hook.StartCoroutine(dialogueRecipient.Download(OnDialogueSheetsDownloaded));
            else OnDialogueSheetsDownloaded(dialogueRecipient.Sheets.ToArray());
        }

        void OnBlockPassed()
        {
            if (!hasBeenBootedUp)
            {
                var characters = Repository.GetAll<Character>(References.Characters);
                var musicianIndex = 0;
                
                foreach (var character in characters)
                {
                    if (character is Interlocutor)
                    {
                        var discardSpot = Repository.GetSingle<Transform>("1.InterlocutorDiscard.0");
                        character.RuntimeLink.transform.position = discardSpot.position;
                    }
                    else
                    {
                        var discardSpot = Repository.GetSingle<Transform>($"1.MusicianDiscard.{musicianIndex}");
                        character.RuntimeLink.transform.position = discardSpot.position;
                        
                        musicianIndex++;
                    }
                }
                
                GoToNextBlock();
                hasBeenBootedUp = true;
            }
            else awaitingCurtain = true;
        }
        void OnDialogueFinished(Dialogue dialogue)
        {
            if (!awaitingCurtain) return;
            hook.StartCoroutine(CurtainRoutine());
        }
        private IEnumerator CurtainRoutine()
        {
            Event.Call(GameEvents.OnCurtainFall);
            
            yield return new WaitForSeconds(1.5f);
            GoToNextBlock();
            yield return new WaitForSeconds(0.5f);
            
            Event.Call(GameEvents.OnCurtainRaised);
            awaitingCurtain = false;

            var phaseHandler = Repository.GetSingle<PhaseHandler>(References.PhaseHandler);
            phaseHandler.Play(PhaseCategory.Dialogue);
        }
        private void GoToNextBlock()
        {
            var block = blocks[GameState.BlockIndex];
            
            block.Execute(previousBlock);
            previousBlock = block;

            Interlocutor = block.Interlocutor;
            Event.Call(GameEvents.OnGoingToNextBlock);
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
                        HandleEventBoundDialogue(row, texts, data);
                        break;
                    
                    case "Harmony":
                        if (TryGetCharacter<Interlocutor>(runtimeSheet["Dialogues", "Source", row], out var interlocutor))  HandleHarmonyDialogue(row, texts, data, interlocutor);
                        break;
                }
            }
            
            GameState.PassBlock();
            Event.Call(GameEvents.OnEncounterBootedUp);
        }

        private bool TryGetCharacter<TChar>(string source, out TChar musician) where TChar : Character
        {
            musician = null;

            if (Enum.TryParse<Actor>(source, out var actor))
            {
                musician = Extensions.GetCharacter<TChar>(actor, true);

                if (musician == null)
                {
                    Debug.LogError($"Could not fetch : {source}");
                    return false;
                }
                else return true;
            }
            else
            {
                Debug.LogError($"Parse fail on : {source}");
                return false;
            }
        }
        
        private void HandleHint(string row, string[] texts, Dictionary<string, string> data, Musician musician)
        {
            if (!data.ContainsKey("Name")) return;
            musician.AddDialogueFailsafe(data["Name"], texts);
        }
        private void HandleAdvance(string row, string[] texts, Dictionary<string, string> data, Musician musician)
        {
            var dialogueNode = new Musician.DialogueNode(row);
            if (!dialogueNode.TryProcess(texts, data))
            {
                Debug.LogError($"Process fail on : {row}");
                return;
            }

            if (data.ContainsKey("Root")) musician.AddDialogueNodeRootKey(row, int.Parse(data["Root"]) - 1);
            musician.AddDialogueNode(dialogueNode);
        }
        
        private void HandleHarmonyDialogue(string row, string[] texts, Dictionary<string, string> data, Interlocutor interlocutor)
        {
            if (!data.ContainsKey("Emotion") || !Enum.TryParse<Emotion>(data["Emotion"], out var emotion)) return;
            
            if (!data.ContainsKey("Block")) return;
            var block = int.Parse(data["Block"]) - 1;
            
            if (!data.ContainsKey("IsCorrect")) return;
            
            if (bool.Parse(data["IsCorrect"]) == true) interlocutor.AddBlockDialogue(row, emotion, texts, block);
            else interlocutor.AddDialogueOption(row, emotion, texts, block);
        }
        
        private void HandleEventBoundDialogue(string row, string[] texts, Dictionary<string, string> data)
        {
            if (!data.ContainsKey("Key")) return;
            
            var eventBoundDialogue = new EventBoundDialogue(row, data["Key"], texts);
            GameState.AddEventBoundDialogue(eventBoundDialogue);
        }
    }
}