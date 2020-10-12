using System;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Prototyping
{
    public class MoveAction : PlayerAction
    {
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
                player.SetTweenMarks(player.Tile.Position, Repository.Get<PlayArea>()[destinationIndex].Position);
            }
            
            base.Execute(beat, offset);
        }
        protected override void ResolveTime(double time, double offset)
        {
            base.ResolveTime(time, offset);
            onTimeResolution.Invoke(time / (actionLength - offset));
        }
    }
}