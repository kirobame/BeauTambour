using System.Linq;
using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class NoteAction : PlayerAction
    {
        protected override ActionType type => ActionType.Standard;
        
        private Tile aim 
        {
            get
            {
                var playArea = Repository.Get<PlayArea>();
                return playArea[0, Repository.Get<Player>().Tile.Index.y];
            }
        }
        
        [HideInInspector] public int sign;
        
        protected override bool CanBeExecuted()
        {
            var roundHandler = Repository.Get<RoundHandler>();
            if (roundHandler.CurrentType != PhaseType.Setup) return false;
            
            if (!aim[TilableType.Musician].Any()) return false;

            var musician = aim[TilableType.Musician].First().Link as Musician;
            return !musician.HasAlreadyPlayed;
        }
        
        protected override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                var musician = aim[TilableType.Musician].First().Link as Musician;
                musician.PlayNote(sign);
            }
               
            base.Execute(beat, offset);
        }
    }
}