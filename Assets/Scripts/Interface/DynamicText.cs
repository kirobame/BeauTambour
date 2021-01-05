using Flux;
using TMPro;
using UnityEngine;
using Event = Flux.Event;

namespace BeauTambour
{
    public class DynamicText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMesh;
        
        [Space, SerializeField] private int sheetIndex;
        [SerializeField] private string key;

        void Start()
        {
            OnLanguageChanged();
            Event.Register(GameEvents.OnLanguageChanged, OnLanguageChanged);
        }

        void OnLanguageChanged()
        {
            var textProvider = Repository.GetSingle<TextProvider>(References.TextProvider);
            if (textProvider.TryGet(sheetIndex, key, out var text)) textMesh.text = text;
        }
    }
}