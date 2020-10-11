using Sirenix.OdinInspector;
using UnityEngine;

namespace Orion.Prototyping
{
    public class IdleTilable : SomeTilable
    {
        [SerializeField] private Token playAreaToken;
        [SerializeField] private Vector2Int startPosition;

        [Button]
        public override void Place()
        {
            var playArea = Repository.Get<PlayArea>(playAreaToken);

            transform.position = playArea[startPosition].Position;
            playArea.Register(this);
        }
    }
}