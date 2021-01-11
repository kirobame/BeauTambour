using System.Collections;
using Flux;
using UnityEngine;

namespace BeauTambour
{
    [CreateAssetMenu(fileName = "neMusicianConversion", menuName = "Beau Tambour/Events/Musician Conversion")]
    public class MusicianConversion : DialogueEvent
    {
        [SerializeField] private Actor target;
        
        [Space, SerializeField] private float exitSpeed;
        [SerializeField] private Spot exitSpot;

        public override void Execute(MonoBehaviour hook) => hook.StartCoroutine(ConversionRoutine(hook));
        private IEnumerator ConversionRoutine(MonoBehaviour hook)
        {
            var complexCharacter = Repository.GetSingle<RuntimeComplexCharacter>($"Complex.{target}");
            yield return hook.StartCoroutine(WalkRoutine(complexCharacter.Asset, exitSpot, exitSpeed));

            complexCharacter.transform.localScale = Vector3.one;
            complexCharacter.Switch();
        }
        
        private IEnumerator WalkRoutine(Character target, Spot destination, float speed)
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