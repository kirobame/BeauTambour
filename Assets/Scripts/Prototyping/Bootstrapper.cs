using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Musician musicianPrefab;
        
        void Start()
        {
            var playArea = Repository.Get<PlayArea>();
            playArea.Generate();
            
            Repository.Get<RythmHandler>().BootUp();

            var player = Instantiate(playerPrefab);
            player.Place(Vector2Int.zero);

            for (var y = 0; y < playArea.Size.y; y++)
            {
                var musician = Instantiate(musicianPrefab);
                musician.Place(new Vector2Int(0, y));
            }
        }
    }
}