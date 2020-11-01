using Flux;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private InputAction inputAction;
        [SerializeField] private int duration;
        [SerializeField] private int startOffset;
        
        private RythmAction action;

        void Start()
        {
            Repository.GetSingle<RythmHandler>(Reference.RythmHandler).BootUp();
            
            inputAction.Enable();
            inputAction.performed += ctxt => Execute();
            
            action = new Interpolation(duration, startOffset, ratio => Debug.Log(ratio));
        }

        private void Execute()
        {
            var rythmHandler = Repository.GetSingle<RythmHandler>(Reference.RythmHandler);
            
            action.Reset();
            Debug.Log(rythmHandler.TryEnqueue(action, Repository.GetSingle<BeauTambourSettings>(Reference.Settings).RythmMarginTolerance));
        }
    }
}