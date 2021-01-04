using UnityEngine;

namespace BeauTambour
{
    public class LanguageHandler : MonoBehaviour
    {
        [ContextMenuItem("Switch", "Switch")]
        [SerializeField] private Language language;

        public void Switch() => GameState.ChangeLanguage(language);
    }
}