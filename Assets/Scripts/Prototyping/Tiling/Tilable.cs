using System;
using UnityEngine;

namespace Orion.Prototyping
{
    public abstract class Tilable : MonoBehaviour, ITilable
    {
        public event Action<ITilable> OnMove;
        
        public TilableType Type => type;
        public Vector2 Position => transform.position;

        public Tile Tile => tile;
        Tile ITilable.Tile
        {
            get => tile;
            set => tile = value;
        }

        [SerializeField] protected TilableType type;
        
        private Tile tile;
        
        public virtual void Place(Vector2Int index)
        {
            var playArea = Repository.Get<PlayArea>();

            transform.position = playArea[index].Position;
            playArea.Register(this);
        }
        
        protected void SendMoveNotification() => OnMove?.Invoke(this);
    }
}