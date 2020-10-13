using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Musician[] musicianPrefabs;
        
        void Start()
        {
            var playArea = Repository.Get<PlayArea>();
            playArea.Generate();
            
            var player = Instantiate(playerPrefab);
            player.Place(Vector2Int.zero);

            for (var y = 0; y < musicianPrefabs.Length; y++)
            {
                var musician = Instantiate(musicianPrefabs[y]);
                musician.Place(new Vector2Int(0, y));
            }
            
            var routine = new WaitForEndOfFrame().ToRoutine();
            routine.Append(new ActionRoutine() {action = () => Repository.Get<SessionHandler>().Begin()});

            StartCoroutine(routine.Call);
        }
    }
}