using System;
using UnityEngine;

namespace Orion.Prototyping
{
    public abstract class SomeTilable : MonoBehaviour, ITilable
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

        protected void NotifyMove() => OnMove?.Invoke(this);
        public abstract void Place();
    }
}