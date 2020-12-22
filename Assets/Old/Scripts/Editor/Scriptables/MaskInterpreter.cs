using System;
using Flux;
using UnityEditor;
using UnityEngine;

namespace Deprecated.Editor
{
    //[CreateAssetMenu(fileName = "NewMaskInterpreter", menuName = "Beau Tambour/Editor/Outcome Interpreters/Mask")]
    public class MaskInterpreter : OutcomeInterpreter
    {
        public override void Interpret(string data, Outcome outcome, Sequencer sequencer)
        {
            var split = data.Split('/');
            
            var serializedObject = new SerializedObject(outcome);
            
            var maskProperty = serializedObject.FindProperty("mask");
            maskProperty.intValue = (int)Enum.Parse(typeof(OutcomeType), split[0]);
            
            var successionMaskProperty = serializedObject.FindProperty("successionMask");
            successionMaskProperty.intValue = (int)Enum.Parse(typeof(OutcomeType), split[1]);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}