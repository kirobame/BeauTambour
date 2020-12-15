using System;
using System.Collections;
using System.Linq;
using Febucci.UI;
using Febucci.UI.Core;
using Flux;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = Flux.Event;

namespace BeauTambour
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private OutcomeType mask;
        [SerializeField] private OutcomeType successionMask;
        
        [ContextMenuItem("Execute", "Process")] [SerializeField]
        private string input;

        public void Process()
        {
            var serializedObject = new SerializedObject(this);
            
            var split = input.Split('/');
            
            var mask = Enum.Parse(typeof(OutcomeType), split[0]);
            Debug.Log(mask);
            
            var maskProperty = serializedObject.FindProperty("mask");
            maskProperty.intValue = (int)mask;
            
            var successionMask = Enum.Parse(typeof(OutcomeType), split[1]);
            Debug.Log(successionMask);
            
            var successionMaskProperty = serializedObject.FindProperty("successionMask");
            successionMaskProperty.intValue = (int)successionMask;

            serializedObject.ApplyModifiedProperties();
        }
    }
}