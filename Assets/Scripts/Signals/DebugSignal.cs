using System.Collections;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "newDebugSignal", menuName = "Beau Tambour/Signals/Debug")]
    public class DebugSignal : Signal
    {
        public override string Category => "debug";
        
        [SerializeField] private float waitTime;

        public override void Execute(MonoBehaviour hook, Character speaker, string[] args) => hook.StartCoroutine(Routine(speaker, args[0]));

        private IEnumerator Routine(Character speaker, string message)
        {
            Debug.Log($"[Debug]:[{speaker.Actor}]:[{Key}] -> {message}");
            
            yield return new WaitForSeconds(waitTime);
            End();
        }
    }
}