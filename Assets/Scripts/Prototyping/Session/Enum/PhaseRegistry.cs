using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    [CreateAssetMenu(fileName = "NewPhaseRegistry", menuName = "Beau Tambour/Registries/Phase")]
    public class PhaseRegistry : Registry<PhaseType, Sprite> { }
}