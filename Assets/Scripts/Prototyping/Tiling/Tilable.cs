using System;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BeauTambour.Prototyping
{
    public abstract class Tilable : SerializedMonoBehaviour, ITilable
    {
        public event Action<ITilable> OnMove;

        public abstract object Link { get; }
        
        public TilableType Type => type;
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Tile Tile => tile;
        Tile ITilable.Tile
        {
            get => tile;
            set => tile = value;
        }

        [SerializeField] protected TilableType type;

        private Tile tile;
        
        [Button]
        public virtual void Place(Vector2Int index)
        {
            var playArea = Repository.Get<PlayArea>();

            transform.position = playArea[index].Position;
            playArea.Register(this);
        }
        
        public void SendMoveNotification() => OnMove?.Invoke(this);
    }
}