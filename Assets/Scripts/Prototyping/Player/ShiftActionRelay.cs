using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class ShiftActionRelay : PlayerActionRelay<BumperAction>
    {
        [SerializeField] private Token musiciansToken;

        public override bool CanBeExecuted() => true;
        
        public override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                var musicians = Repository.GetStack<Musician>(musiciansToken);
                foreach (var musician in musicians) musician.PrepareShift(source.Direction);
            }
        }
        public override void ResolveTime(double time, double offset)
        {
            var musicians = Repository.GetStack<Musician>(musiciansToken);
            foreach (var musician in musicians) musician.Shift(time / (source.ActionLength - offset));
        }
    }
}