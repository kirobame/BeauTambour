using System.Collections;
using System.Linq;
using Febucci.UI;
using Febucci.UI.Core;
using Flux;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Helper : MonoBehaviour
    {
        [ContextMenuItem("Execute", "Process")]
        [SerializeField] private TextAsset asset;

        public void Process()
        {
            var sheet = new Sheet();
            sheet.Process(asset.text);

            var runtimeSheet = new RuntimeSheet();
            runtimeSheet.Process(sheet);

            foreach (var key in runtimeSheet.ColumnKeys["Dialogues"])
            {
                Debug.Log(key);
            }            
            
            Debug.Log( runtimeSheet["Dialogues", SupportedLanguage.Français.ToString(), "S1"]);
            
            var data = runtimeSheet["Dialogues", SupportedLanguage.Français.ToString(), "S1"];
            var dialogue = Dialogue.Parse(data);
           
            Debug.Log(data);
            Debug.Log(dialogue);
        }
    }
}