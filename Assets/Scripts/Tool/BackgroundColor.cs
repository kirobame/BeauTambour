using UnityEngine;

namespace BeauTambour.Tooling
{
    public class BackgroundColor : MonoBehaviour, IColorable
    {
        [SerializeField] private Camera camera;

        public void SetColor(Color color) => camera.backgroundColor = color;
    }
}