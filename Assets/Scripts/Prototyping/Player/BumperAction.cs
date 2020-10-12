using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class BumperAction : PlayerAction
    {
        public int Direction => direction;
        
        [SerializeField] private int direction;

        private PlayerActionRelay<BumperAction> relay;
        
        protected override ActionType type => ActionType.Standard;

        public void SetRelay(PlayerActionRelay<BumperAction> relay) => this.relay = relay;
        
        protected override bool CanBeExecuted() => relay.CanBeExecuted();
        
        protected override void Execute(int beat, double offset)
        {
            Debug.Log(relay.GetType());
            relay.Execute(beat, offset);
            base.Execute(beat, offset);
        }
        protected override void ResolveTime(double time, double offset)
        {
            base.ResolveTime(time, offset);
            relay.ResolveTime(time, offset);
        }
    }
}