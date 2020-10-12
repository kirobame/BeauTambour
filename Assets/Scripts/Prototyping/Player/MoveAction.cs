using System;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Prototyping
{
    public class MoveAction : PlayerAction, ITweenable<Vector2>
    {
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }
        
        protected override ActionType type => ActionType.Standard;

        [PropertyOrder(-1)] public OrionEvent<double> onTimeResolution = new OrionEvent<double>();

        [SerializeField] private Vector2Int direction;
        
        protected override bool CanBeExecuted()
        {
            var destinationIndex = player.Tile.Index + direction;
            return player.IndexedBounds.Contains(destinationIndex);
        }

        protected override void Execute(int beat, double offset)
        {
            var destinationIndex = player.Tile.Index + direction;
            if (beat == 0)
            {
                Start = player.Tile.Position;
                End = Repository.Get<PlayArea>()[destinationIndex].Position;
            }
            base.Execute(beat, offset);
        }
        protected override void ResolveTime(double time, double offset)
        {
            base.ResolveTime(time, offset);
            onTimeResolution.Invoke(time / (actionLength - offset));
        }

        void ITweenable<Vector2>.Apply(Vector2 position)
        {
            player.Position = position;
            player.ActualizeTiling();
        }
    }
}