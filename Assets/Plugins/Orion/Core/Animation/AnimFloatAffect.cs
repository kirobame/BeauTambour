using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Orion
{
    public struct AnimFloatAffect : IAnimatorAffect
    {
        #if UNITY_EDITOR

        [SerializeField] private RuntimeAnimatorController controller;
        
        private IEnumerable GetFloats()
        {
            var list = new ValueDropdownList<int>();
            list.Add("Null", 0);
            
            if (controller == null) return list;
            
            var editorController = controller as AnimatorController;
            foreach (var parameter in editorController.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Float) list.Add(parameter.name, parameter.nameHash);
            }
            return list;
        }
        #endif
        
        [ValueDropdown("GetFloats")]
        [SerializeField] private int floatId;

        [SerializeField] private float value;

        public void Play(Animator animator) => animator.SetFloat(floatId, value);
    }
}