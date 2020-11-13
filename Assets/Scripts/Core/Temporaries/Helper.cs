using System.Linq;
using Flux;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMesh;

        void Update()
        {
            Debug.Log($"{textMesh.textInfo.lineInfo[0].maxAdvance} / {textMesh.textInfo.lineCount}");
        }
    }
}