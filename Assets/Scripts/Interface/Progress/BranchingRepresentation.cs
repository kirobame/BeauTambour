using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
                
                var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                branches[i].localRotation = rotation;
                branches[i].GetChild(0).localRotation = Quaternion.Inverse(rotation);
            }

            for (var i = count; i < branches.Length; i++) branches[i].gameObject.SetActive(false);
        }

        public IEnumerator SelectionRoutine(int index)
        {
            var time = 0.0f;
            var duration = 0.15f;

            var count = branches.Count(rect => rect.gameObject.activeInHierarchy);
            if (count > 1)
            {
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
            }
            
            var angle = -(separation * (count -1)) / 2.0f + separation * index;
            if (angle != 0)
            {
                var icon = branches[index].GetChild(0) as RectTransform;
                var startingRotation = branches[index].localRotation;
                
                time = 0.0f;
                while (time < duration)
                {
                    Execute(time / duration);
                    
                    yield return new WaitForEndOfFrame();
                    time += Time.deltaTime;
                }
                Execute(1.0f);

                void Execute(float ratio)
                {
                    var rotation = Quaternion.Lerp(startingRotation, Quaternion.identity, ratio);
                    branches[index].localRotation = rotation;
                    icon.localRotation = Quaternion.Inverse(rotation);
                }
            }
        }
        public IEnumerator DisapparitionRoutine(Image link)
        {
            var time = 0.0f;
            var duration = 0.3f;

            while (time < duration)
            {
                Execute(time / duration);
                
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
            Execute(1.0f);

            void Execute(float ratio)
            {
                link.transform.localScale = new Vector3(1, ratio, 0);
                foreach (var branch in branches)
                {
                    var scale = branch.localScale;
                    scale.y = 1.0f - ratio;

                    branch.localScale = scale;
                }
            }
        }
    }
}