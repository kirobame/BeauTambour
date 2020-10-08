using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private Token rythmHandlerToken;
        
        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.P)) return;
            
            var success = Repository.Get<RythmHandler>(rythmHandlerToken).TryEnqueue(Print);
            Debug.Log(success);
        }

        private void Print(double beat) => Debug.Log($"Callback : {beat}");
    }
}