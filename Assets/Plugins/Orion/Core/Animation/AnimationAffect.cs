using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;

namespace Orion
{
    public struct AnimationAffect : IAnimatorAffect
    {
        #if UNITY_EDITOR

        [SerializeField] private RuntimeAnimatorController controller;
        
        private IEnumerable GetAnimations()
        {
            var list = new ValueDropdownList<int>();
            list.Add("Null", 0);
            
            if (controller == null) return list;

            var editorController = controller as AnimatorController;
            foreach (var layer in editorController.layers)
            {
                foreach (var stateWrapper in layer.stateMachine.states) list.Add(stateWrapper.state.name, stateWrapper.state.nameHash);
            }
            
            return list;
        }
        #endif
        
        [ValueDropdown("GetAnimations")]
        [SerializeField] private int animationId;

        public void Play(Animator animator) => animator.Play(animationId);
    }
}