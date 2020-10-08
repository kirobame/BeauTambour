using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Orion
{
    public struct AnimTriggerAffect : IAnimatorAffect
    {
        #if UNITY_EDITOR

        [SerializeField] private RuntimeAnimatorController controller;
        
        private IEnumerable GetTriggers()
        {
            var list = new ValueDropdownList<int>();
            list.Add("Null", 0);
            
            if (controller == null) return list;
            
            var editorController = controller as AnimatorController;
            foreach (var parameter in editorController.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger) list.Add(parameter.name, parameter.nameHash);
            }
            return list;
        }
        #endif
        
        [ValueDropdown("GetTriggers")]
        [SerializeField] private int triggerId;
        
        public void Play(Animator animator) => animator.SetTrigger(triggerId);
    }
}