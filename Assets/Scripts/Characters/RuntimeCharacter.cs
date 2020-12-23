using Flux;
using UnityEngine;

namespace BeauTambour
{
    [IconIndicator(-3795152337350425143)]
    public class RuntimeCharacter : MonoBehaviour
    {
        [SerializeField] private Character asset;

        void Awake() => asset.Bootup(this);
    }
}