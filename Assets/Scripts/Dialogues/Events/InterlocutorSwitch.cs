using System.Collections;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "newDialogueEvent", menuName = "Beau Tambour/Events/Interlocutor Switch")]
    public class InterlocutorSwitch : DialogueEvent
    {
        [SerializeField] private Interlocutor interlocutor;
        
        [Space, SerializeField] private float exitSpeed;
        [SerializeField] private Spot exitSpot;
        
        [Space, SerializeField] private float entrySpeed;
        [SerializeField] private Spot entrySpot;

        public override void Execute(MonoBehaviour hook) => hook.StartCoroutine(SwitchRoutine(hook));
        private IEnumerator SwitchRoutine(MonoBehaviour hook)
        {
            var encounter = Repository.GetSingle<Encounter>(References.Encounter);
            var exitingInterlocutor = encounter.Interlocutor;

            yield return hook.StartCoroutine(WalkRoutine(exitingInterlocutor, exitSpot, exitSpeed));
            
            encounter.ChangeInterlocutor(interlocutor);
            interlocutor.RuntimeLink.Reinitialize();

            yield return hook.StartCoroutine(WalkRoutine(interlocutor, entrySpot, entrySpeed));

            End();
        }

        private IEnumerator WalkRoutine(Interlocutor target, Spot destination, float speed)
        {
            var start = target.RuntimeLink.transform.position;
            var end = Repository.GetSingle<Transform>(destination).position;
            
            var distance = Vector2.Distance(start, end);
            var direction = (end - start).normalized;
            
            var initialDirection = direction;
            while (distance > 0.1f && direction.x == initialDirection.x)
            {
                var delta = direction.normalized * (speed * Time.deltaTime);
                target.RuntimeLink.transform.position += delta;
                
                yield return new WaitForEndOfFrame();
                
                distance = Vector2.Distance(target.RuntimeLink.transform.position, end);
                direction = (end - target.RuntimeLink.transform.position).normalized;
            }
            target.RuntimeLink.transform.position = Vector3.Lerp(start, end, 1);
        }
    }
}