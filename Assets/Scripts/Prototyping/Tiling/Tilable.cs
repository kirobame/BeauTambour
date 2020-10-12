using System;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BeauTambour.Prototyping
{
    public abstract class Tilable : SerializedMonoBehaviour, ITilable, ITweenable<Vector2>
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
        
        public Vector2 Onset { get; protected set; }
        public Vector2 Outset { get; protected set; }

        [SerializeField] protected TilableType type;

        private Tile tile;
        
        [Button]
        public virtual void Place(Vector2Int index)
        {
            var playArea = Repository.Get<PlayArea>();

            transform.position = playArea[index].Position;
            playArea.Register(this);
        }

        public void SetTweenMarks(Vector2 start, Vector2 end)
        {
            Onset = start;
            Outset = end;
        }

        public void ActualizeTiling() => OnMove?.Invoke(this);
        
        void ITweenable<Vector2>.Apply(Vector2 position)
        {
            Position = position;
            ActualizeTiling();
        }
    }
}