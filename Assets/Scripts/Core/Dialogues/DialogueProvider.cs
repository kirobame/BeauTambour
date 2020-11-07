using System.Collections;
using System.Linq;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class DialogueProvider : MonoBehaviour
    {
        [SerializeField] private CSVRecipient dialogues;
        [SerializeField] private bool useBackup;

        private EncounterSheet[] encounterSheets;

        void Start()
        {
            if (!useBackup) StartCoroutine(dialogues.Download(Process));
            else Process(dialogues.Sheets.ToArray());
        }

        public void Process(Sheet[] sheets)
        {
            encounterSheets = new EncounterSheet[sheets.Length];
            for (var i = 0; i < sheets.Length; i++)
            {
                var encounterSheet = new EncounterSheet();
                encounterSheet.Process(sheets[i]);

                encounterSheets[i] = encounterSheet;
            }
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

            return encounterSheets[encounterIndex].TryGet("Dialogues", language.ToString(), id, out dialogue);
        }
    }
}