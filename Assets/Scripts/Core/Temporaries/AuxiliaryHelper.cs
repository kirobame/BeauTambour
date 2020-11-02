using UnityEngine;

namespace BeauTambour
{
    public class AuxiliaryHelper : MonoBehaviour
    {
        public void Display(string message) => Debug.Log($"Received in auxiliary Helper : {message}");
    }
}