using System.Collections;
using UnityEngine;

namespace BeauTambour
{
    public class BranchingRepresentation : MonoBehaviour
    {
        public RectTransform RectTransform => transform as RectTransform;
        
        [SerializeField] private RectTransform[] branches;
        [SerializeField] private float separation;

        private int count;

        public void Prepare(int count)
        {
            this.count = count;
            var start = -(separation * (count - 1)) / 2.0f;

            for (var i = 0; i < count; i++)
            {
                var angle = start + separation * i;

                branches[i].localScale = Vector3.one;
                branches[i].gameObject.SetActive(true);
                
                branches[i].localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            for (var i = count; i < branches.Length; i++) branches[i].gameObject.SetActive(false);
        }

        public IEnumerator SelectionRoutine(int index)
        {
            var time = 0.0f;
            var duration = 0.15f;

            while (time < duration)
            {
                HideBranches(time / duration);
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            HideBranches(1.0f);
            
            void HideBranches(float ratio)
            {
                for (var i = 0; i < branches.Length; i++)
                {
                    if (i == index) continue;
                    branches[i].localScale = Vector3.Lerp(Vector3.one, Vector3.zero, ratio);
                }
            }
            
            var angle = -(separation * (count -1)) / 2.0f + separation * index;
            if (angle != 0)
            {
                var startingRotation = branches[index].localRotation;
                
                time = 0.0f;
                while (time < duration)
                {
                    branches[index].localRotation = Quaternion.Lerp(startingRotation, Quaternion.identity, time / duration);
                    
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                branches[index].localRotation = Quaternion.Lerp(startingRotation, Quaternion.identity, 1.0f);
            }
        }
    }
}