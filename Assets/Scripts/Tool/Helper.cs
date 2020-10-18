using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Tooling
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private InputActionReference actionReference;

        void Awake()
        {
            actionReference.action.performed += ctxt => Debug.Log(ctxt.ReadValueAsObject());
        }
        
        void OnEnable() 
        {
            actionReference.asset.Enable();
            actionReference.action.actionMap.Enable();
        }
        void OnDisable()
        {
            actionReference.asset.Disable();
            actionReference.action.actionMap.Disable();
        }
    }
}