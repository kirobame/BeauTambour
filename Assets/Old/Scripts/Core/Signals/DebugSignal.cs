using System.Collections;
using BeauTambour;
using UnityEngine;

namespace Deprecated
{
    //[CreateAssetMenu(fileName = "newDebugSignal", menuName = "Beau Tambour/Signals/Debug")]
    /*public class DebugSignal : Signal
    {
        public override string Category => "debug";
        
        [SerializeField] private float duration;

        public override void Execute(MonoBehaviour hook, Character character, string[] args)
        {
            hook.StartCoroutine(Routine(args[0]));
        }

        private IEnumerator Routine(string message)
        {
            Debug.Log($"EVENT {name} : Start of Debug event : " + message);
            yield return new WaitForSeconds(duration);

            End();
            Debug.Log($"EVENT {name} : end");
        }
    }*/
}