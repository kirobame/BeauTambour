using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    [CreateAssetMenu(fileName = "NewShapeRegistry", menuName = "Beau Tambour/Registries/Shape")]
    public class ShapeRegistry : Registry<Shape, Sprite> { }
}