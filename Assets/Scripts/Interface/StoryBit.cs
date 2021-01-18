using Febucci.UI;
using Flux;
using TMPro;
using UnityEngine;

namespace BeauTambour
{
    public class StoryBit : MonoBehaviour
    {
        public TMP_Text TextMesh => textMesh;
        [SerializeField] private TMP_Text textMesh;

        public TextAnimatorPlayer Player => player;
        [SerializeField] private TextAnimatorPlayer player;

        private string text;
        
        public void Execute(int sheetIndex, string key)
        {
            var textProvider = Repository.GetSingle<TextProvider>(References.TextProvider);
            textProvider.TryGet(sheetIndex, key, out text);

            textMesh.text = text;

            var color = textMesh.color;
            color.a = 0f;
            textMesh.color = color;
        }
        public void Show()
        {
            var color = textMesh.color;
            color.a = 1f;
            textMesh.color = color;
            
            player.StopShowingText();
            
            if (text == string.Empty) player.ShowText("Text fetching error");
            else player.ShowText(text);
            
            player.StartShowingText();
        }
    }
}