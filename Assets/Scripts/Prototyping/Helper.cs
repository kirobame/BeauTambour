using Orion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private Token rythmHandlerToken;
        [SerializeField, Min(1)] private int duration = 1;

        [Button]
        void Subscribe() => Repository.Get<RythmHandler>(rythmHandlerToken).OnBeat += Callback;

        private void Callback(double beat)
        {
            var rythmHandler = Repository.Get<RythmHandler>(rythmHandlerToken);

            rythmHandler.OnBeat -= Callback;
            rythmHandler.TryStandardEnqueue(PrintTime, duration);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                var success = Repository.Get<RythmHandler>(rythmHandlerToken).TryStandardEnqueue(PrintTime, duration);
                Debug.Log($"Standard Enqueue success : {success}");
            }
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                var success = Repository.Get<RythmHandler>(rythmHandlerToken).TryPlainEnqueue(PrintBeat, duration);
                Debug.Log($"Plain  Enqueue success : {success}");
            }
        }

        private void PrintBeat(int beat) => Debug.Log($"Beat : {beat}");
        private void PrintTime(double time)
        {
            var seconds = Repository.Get<RythmHandler>(rythmHandlerToken).SecondsPerBeats * time;
            Debug.Log($"Time : {time} / {seconds}");
        }
    }
}