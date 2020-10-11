using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion.Prototyping
{
    public class MovingTilable : SomeTilable
    {
        [SerializeField] private Token playAreaToken;

        [Button]
        public override void Place()
        {
            var playArea = Repository.Get<PlayArea>(playAreaToken);

            transform.position = playArea[0, 0].Position;
            playArea.Register(this);
        }

        void Update()
        {
            var input = Vector2Int.zero;

            if (Input.GetKeyDown(KeyCode.RightArrow)) input.x = 1;
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) input.x = -1;
            else if (Input.GetKeyDown(KeyCode.UpArrow)) input.y = 1;
            else if (Input.GetKeyDown(KeyCode.DownArrow)) input.y = -1;

            if (input == Vector2Int.zero) return;
            
            var playArea = Repository.Get<PlayArea>(playAreaToken);
            var index = Tile.Index + input;
            
            if (playArea.IndexedBounds.Contains(index) && !playArea[index][TilableType.NoteFragment].Any())
            {
                transform.position = playArea[index].Position;
                NotifyMove();
            }
        }
    }
}