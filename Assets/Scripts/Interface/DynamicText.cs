using Flux;
using TMPro;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class DynamicText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private bool bootup = true;
        
        [Space, SerializeField] private int sheetIndex;
        [SerializeField] private string key;

        void Start()
        {
            if (!bootup) return;
            
            Refresh();
            Event.Register(GameEvents.OnLanguageChanged, Refresh);
        }

        public void SetText(string key)
        {
            this.key = key;
            Refresh();
        }

        void Refresh()
        {
            var textProvider = Repository.GetSingle<TextProvider>(References.TextProvider);
            if (textProvider.TryGet(sheetIndex, key, out var text)) textMesh.text = text;
        }
    }
}