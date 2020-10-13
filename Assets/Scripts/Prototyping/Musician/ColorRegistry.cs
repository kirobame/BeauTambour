using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    [CreateAssetMenu(fileName = "NewColorRegistry", menuName = "Beau Tambour/Registries/Color")]
    public class ColorRegistry : Registry<Color, UnityEngine.Color> { }
}