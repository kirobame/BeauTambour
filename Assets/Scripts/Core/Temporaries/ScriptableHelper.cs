using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "NewHelper", menuName = "Beau Tambour/Helper")]
    public class ScriptableHelper : ScriptableObject
    {
        [SerializeField] private string text;
        [SerializeField] private int[] numbers;
        [SerializeField] private int number;
    }
}