using Flux;
using TMPro;
using UnityEngine;

namespace BeauTambour
{
    public class DynamicText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMesh;
        
        [Space, SerializeField] private int sheetIndex;
        [SerializeField] private string key;

        void Start()
        {
            var textProvider = Repository.GetSingle<TextProvider>(References.TextProvider);
            if (textProvider.TryGet(sheetIndex, key, out var text)) textMesh.text = text;
        }
    }
}