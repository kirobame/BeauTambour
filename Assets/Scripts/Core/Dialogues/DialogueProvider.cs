using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class DialogueProvider : MonoBehaviour
    {
        #region Encapsulated Types

        public enum EventType
        {
            OnDialoguesDownloaded
        }
        #endregion
        
        public Dialogue this[DialogueReference reference]
        {
            get
            {
                var settings = Repository.GetSingle<RuntimeSettings>(Reference.RuntimeSettings);
                return Dialogue.Parse(encounterSheets[reference.EncounterIndex - 1]["Dialogues", settings.Language.ToString(), reference.Id]);
            }
        }

        public IReadOnlyList<RuntimeSheet> Sheets => encounterSheets;
        
        [SerializeField] private CSVRecipient dialogues;
        [SerializeField] private bool useBackup;

        private RuntimeSheet[] encounterSheets;

        void Awake() => Event.Open(EventType.OnDialoguesDownloaded);
        void Start()
        {
            if (!useBackup) StartCoroutine(dialogues.Download(Process));
            else Process(dialogues.Sheets.ToArray());
        }

        public void Process(Sheet[] sheets)
        {
            encounterSheets = new RuntimeSheet[sheets.Length];
            for (var i = 0; i < sheets.Length; i++)
            {
                var encounterSheet = new RuntimeSheet();
                encounterSheet.Process(sheets[i]);

                encounterSheets[i] = encounterSheet;
            }
            
            if (Application.isPlaying) Event.Call(EventType.OnDialoguesDownloaded);
        }

        public bool TryGetDialogue(DialogueReference reference, out Dialogue dialogue) => TryGetDialogue(reference.EncounterIndex - 1, reference.Id, out dialogue);
        public bool TryGetDialogue(int encounterIndex, string id, out Dialogue dialogue)
        {
            var settings = Repository.GetSingle<RuntimeSettings>(Reference.RuntimeSettings);
            return TryGetDialogue(encounterIndex, id, settings.Language, out dialogue);
        }
        public bool TryGetDialogue(int encounterIndex, string id, SupportedLanguage language, out Dialogue dialogue)
        {
            dialogue = default;
            if (encounterIndex < 0 || encounterIndex >= encounterSheets.Length) return false;

            if (encounterSheets[encounterIndex].TryGet("Dialogues", language.ToString(), id, out var text))
            {
                dialogue = Dialogue.Parse(text);
                return true;
            }
            else return false;
        }
    }
}