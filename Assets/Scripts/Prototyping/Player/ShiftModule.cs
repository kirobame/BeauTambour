using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class ShiftModule : Module<bool>
    {
        [SerializeField] private Player player;
        [SerializeField] private Token musicianGroupToken;
        
        [SerializeField] private int direction;
        
        protected override void OnActionStarted(bool input)
        {
            if (player.IsCurrentBeatClaimed || !Repository.Get<RythmHandler>().IsOnTempo()) return;

            player.ClaimCurrentBeat();
            
            var musicians = Repository.GetStack<Musician>(musicianGroupToken);
            foreach (var musician in musicians) musician.Shift(direction);
        }
        protected override void OnAction(bool input) { }
        protected override void OnActionEnded(bool input) { }
    }
}