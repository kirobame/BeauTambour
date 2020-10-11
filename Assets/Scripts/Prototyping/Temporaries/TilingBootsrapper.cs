using System.Collections.Generic;
using UnityEngine;

namespace Orion.Prototyping
{
    public class TilingBootsrapper : MonoBehaviour
    {
        [SerializeField] private MovingTilable playerPrefab;
        [SerializeField] private IdleTilable obstaclePrefab;
        [SerializeField] private int obstacleCount;
        
        void Start()
        {
            var playArea = Repository.Get<PlayArea>();
            playArea.Generate();

            var placements = new List<Vector2Int>();
            var row = 1;
            
            for (var x = 0; x < playArea.Size.x; x++)
            {
                for (var y = 1; y < playArea.Size.y; y++) placements.Add(new Vector2Int(x,y));
                row = 0;
            }

            for (var i = 0; i < obstacleCount; i++)
            {
                var index = Random.Range(0, placements.Count);
                var obstacle = Instantiate(obstaclePrefab);
                obstacle.Place(placements[index]);
                
                placements.RemoveAt(index);
            }

            var player = Instantiate(playerPrefab);
            player.Place(new Vector2Int(0,0));
        }
    }
}