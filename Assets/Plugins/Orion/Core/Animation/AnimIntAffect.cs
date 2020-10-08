using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Orion
{
    public struct AnimIntAffect : IAnimatorAffect
    {
        #if UNITY_EDITOR

        [SerializeField] private RuntimeAnimatorController controller;
        
        private IEnumerable GetIntegers()
        {
            var list = new ValueDropdownList<int>();
            list.Add("Null", 0);
            
            if (controller == null) return list;
            
            var editorController = controller as AnimatorController;
            foreach (var parameter in editorController.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Int) list.Add(parameter.name, parameter.nameHash);
            }
            return list;
        }
        #endif
        
        [ValueDropdown("GetIntegers")]
        [SerializeField] private int integerId;

        [SerializeField] private int value;

        public void Play(Animator animator) => animator.SetInteger(integerId, value);
    }
}