using System;
using System.Linq;
using Orion;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeauTambour.Prototyping
{
    public class StickModule : Module<Vector2>
    {
        [SerializeField] private float reactionTime;
        [SerializeField, Range(0f, 1f)] private float wantedMagnitude;

        [Space, SerializeField] private MoveAction moveAction;
        
        private bool isActive;
        private float time;
        
        protected override void OnActionStarted(Vector2 input) => isActive = true;
        protected override void OnAction(Vector2 input)
        {
            var magnitude = input.magnitude;
            if (isActive && magnitude >= wantedMagnitude && time <= reactionTime)
            {
                var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

                var direction = Vector2Int.zero;
                if (angle >= 0 && angle < 45 || angle <= 0 && angle > -45)
                {
                    direction = Vector2Int.right;
                }
                else if (angle >= 45 && angle <= 135)
                {
                    direction = Vector2Int.up;
                }
                else if (angle > 135 && angle <= 180 || angle >= -180 && angle < -135)
                {
                    direction = Vector2Int.left;
                }
                else direction = Vector2Int.down;

                moveAction.direction = direction;
                moveAction.TryBeginExecution();
                
                isActive = false;
            }
            
            time += Time.deltaTime;
        }

        protected override void OnActionEnded(Vector2 input)
        {
            time = 0f;
            isActive = false;
        }

        private void Update()
        {
            if (isActive) time += Time.deltaTime;
        }
    }

    public class MoveAction : PlayerAction
    {
        protected override ActionType type => ActionType.Standard;
        
        [PropertyOrder(-1)] public OrionEvent<double> onTimeResolution = new OrionEvent<double>();

        [HideInInspector] public Vector2Int direction;
        
        protected override bool CanBeExecuted()
        {
            var destinationIndex = player.Tile.Index + direction;
            if (!player.IndexedBounds.Contains(destinationIndex)) return false;

            return direction != Vector2Int.zero && destinationIndex.x < Repository.Get<BlockGenerator>().StartX - 1;
        }

        protected override void Execute(int beat, double offset)
        {
            var destinationIndex = player.Tile.Index + direction;
            if (beat == 0)
            {
                player.SetTweenMarks(player.Tile.Position, Repository.Get<PlayArea>()[destinationIndex].Position);
            }
            if (beat == actionLength) direction = Vector2Int.zero;
            
            base.Execute(beat, offset);
        }
        protected override void ResolveTime(double time, double offset)
        {
            base.ResolveTime(time, offset);
            onTimeResolution.Invoke(time / (actionLength - offset));
        }
    }
}