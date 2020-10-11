using System;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Prototyping
{
    public class MoveModule : Module<Vector2>
    {
        [SerializeField] private Player player;
        [SerializeField] private IProxy<AnimationCurve> curveProxy;
        private AnimationCurve curve
        {
            get => curveProxy.Read();
            set => curveProxy.Write(value);
        }

        protected override void OnActionStarted(Vector2 input)
        {
            var value = Vector2Int.zero;
            
            if (input.x != 0 && input.y == 0) value = new Vector2Int(Mathf.RoundToInt(input.x), 0);
            else if (input.x == 0 && input.y != 0) value = new Vector2Int(0,Mathf.RoundToInt(input.y));

            if (value == Vector2Int.zero) return;

            var destinationIndex = player.Tile.Index + value;
            if (player.IsCurrentBeatClaimed || !IsDestinationValid(destinationIndex)) return;
            
            var start = player.Tile;
            var end = Repository.Get<PlayArea>()[destinationIndex];
            
            var rythmHandler = Repository.Get<RythmHandler>();
            if (rythmHandler.TryStandardEnqueue(time => Move(time, start , end), 1))
            {
                player.ClaimCurrentBeat();
            }
        }
        protected override void OnAction(Vector2 input) { }
        protected override void OnActionEnded(Vector2 input) { }

        private bool IsDestinationValid(Vector2Int destinationIndex)
        {
            return player.IndexedBounds.Contains(destinationIndex);
        }

        private void Move(double time, Tile start, Tile end)
        {
            player.Position = Vector2.Lerp(start.Position, end.Position, curve.Evaluate((float)time));
            player.SendMoveNotification();
        }
    }
}