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
                return Dialogue.Parse(encounterSheets[reference.EncounterId]["Dialogues", settings.Language.ToString(), reference.Id]);
            }
        }

        public System.Collections.Generic.KeyValuePair<string, RuntimeSheet>[] Sheets => encounterSheets.ToArray();
        
        [SerializeField] private CSVRecipient dialogues;
        [SerializeField] private bool useBackup;

        private Dictionary<string, RuntimeSheet> encounterSheets;

        void Awake() => Event.Open(EventType.OnDialoguesDownloaded);
        void Start()
        {
            if (!useBackup) StartCoroutine(dialogues.Download(Process));
            else Process(dialogues.Sheets.ToArray());
        }

        public void Process(Sheet[] sheets)
        {
            encounterSheets = new Dictionary<string, RuntimeSheet>();
            for (var i = 0; i < sheets.Length; i++)
            {
                var encounterSheet = new RuntimeSheet();
                encounterSheet.Process(sheets[i]);
                
                encounterSheets.Add(sheets[i].Name, encounterSheet);
            }
            
            if (Application.isPlaying) Event.Call(EventType.OnDialoguesDownloaded);
        }

        public bool TryGetDialogue(DialogueReference reference, out Dialogue dialogue) => TryGetDialogue(reference.EncounterId, reference.Id, out dialogue);
        public bool TryGetDialogue(string encounterId, string id, out Dialogue dialogue)
        {
            var settings = Repository.GetSingle<RuntimeSettings>(Reference.RuntimeSettings);
            return TryGetDialogue(encounterId, id, settings.Language, out dialogue);
        }
        public bool TryGetDialogue(string encounterId, string id, SupportedLanguage language, out Dialogue dialogue)
        {
            dialogue = default;
            if (!encounterSheets.TryGetValue(encounterId, out RuntimeSheet runtimeSheet)) return false;
            
            if (runtimeSheet.TryGet("Dialogues", language.ToString(), id, out var text))
            {
                dialogue = Dialogue.Parse(text);
                return true;
            }
            else return false;
        }
    }
}