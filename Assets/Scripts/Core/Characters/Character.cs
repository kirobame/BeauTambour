using Flux;
using TMPro;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public abstract class Character : ScriptableObject
    {
        public RuntimeCharacter Instance => Repository.GetSingle<RuntimeCharacter>(actor);

        public Actor Name => actor;
        [SerializeField] private Actor actor;

        public Anchor Anchor => anchor;
        [SerializeField] private Anchor anchor;

        public TMP_FontAsset Font => font;
        [SerializeField] private TMP_FontAsset font;

        public Color FontColor => fontColor;
        [SerializeField] private Color fontColor;

        public Color BackgroundColor => backgroundColor;
        [SerializeField] private Color backgroundColor;
    }
}