using System.Linq;
using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class ShiftAction : PlayerAction
    {
        [SerializeField] private Token musiciansToken;
        [SerializeField] private int direction;
        
        protected override ActionType type => ActionType.Standard;
        
        protected override bool CanBeExecuted() => true;
        
        protected override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                var musicians = Repository.GetStack<Musician>(musiciansToken);
                foreach (var musician in musicians) musician.PrepareShift(direction);
            }
            base.Execute(beat, offset);
        }
        protected override void ResolveTime(double time, double offset)
        {
            base.ResolveTime(time, offset);
            
            var musicians = Repository.GetStack<Musician>(musiciansToken);
            foreach (var musician in musicians) musician.Shift(time / (actionLength - offset));
        }
    }

    public class NoteAction : PlayerAction
    {
        protected override ActionType type => ActionType.Standard;

        protected override bool CanBeExecuted()
        {
            var playArea = Repository.Get<PlayArea>();
            var tile = playArea[0, player.Tile.Index.y];

            return tile[TilableType.Musician].Any();
        }

        protected override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                
            }
            
            base.Execute(beat, offset);
        }
    }
}