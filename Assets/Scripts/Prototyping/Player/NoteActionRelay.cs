using System.Linq;
using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class NoteActionRelay : PlayerActionRelay<BumperAction>
    {
        private Tile aim 
        {
            get
            {
                var playArea = Repository.Get<PlayArea>();
                return playArea[0, Repository.Get<Player>().Tile.Index.y];
            }
        }
        
        public override bool CanBeExecuted() => aim[TilableType.Musician].Any();

        public override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                var musician = aim[TilableType.Musician].First().Link as Musician;
                musician.PlayNote(source.Direction);
            }
        }
        public override void ResolveTime(double time, double offset) { }
    }
}