using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Orion
{
    public struct AnimBoolAffect : IAnimatorAffect
    {
        #if UNITY_EDITOR

        [SerializeField] private RuntimeAnimatorController controller;
        
        private IEnumerable GetBooleans()
        {
            var list = new ValueDropdownList<int>();
            list.Add("Null", 0);
            
            if (controller == null) return list;
            
            var editorController = controller as AnimatorController;
            foreach (var parameter in editorController.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool) list.Add(parameter.name, parameter.nameHash);
            }
            return list;
        }
        #endif
        
        [ValueDropdown("GetBooleans")]
        [SerializeField] private int booleanId;

        [SerializeField] private bool value;

        public void Play(Animator animator) => animator.SetBool(booleanId, value);
    }
}