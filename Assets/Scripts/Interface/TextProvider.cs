using Flux;
using UnityEngine;

namespace BeauTambour
{
    public class TextProvider : MonoBehaviour
    {
        [SerializeField] private CSVRecipient textSheets;
        private RuntimeSheet[] runtimeSheets;
        
        void Awake()
        {
            Repository.Reference(this, References.TextProvider);

            runtimeSheets =  new RuntimeSheet[textSheets.Sheets.Count];
            for (var i = 0; i < runtimeSheets.Length; i++)
            { 
                var runtimeSheet = new RuntimeSheet();
                runtimeSheet.Process(textSheets.Sheets[i]);

                runtimeSheets[i] = runtimeSheet;
            }
        }

        public bool TryGet(int sheetIndex, string key, out string text)
        {
            text = string.Empty;
            if (sheetIndex < 0 || sheetIndex >= runtimeSheets.Length) return false;

            if (!runtimeSheets[sheetIndex].RowKeys["Data"].Contains(key)) return false;

            text = runtimeSheets[sheetIndex]["Data", GameState.UsedLanguage.ToString(), key];
            return true;
        }
    }
}