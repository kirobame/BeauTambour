using Orion;
using UnityEngine;

namespace BeauTambour.Prototyping
{
    public class ConfirmAction : PlayerAction
    {
        protected override ActionType type => ActionType.Standard;

        [SerializeField] private BumperAction[] targets = new BumperAction[2];
        [SerializeField] private PlayerActionRelay<BumperAction>[] firstRelays = new PlayerActionRelay<BumperAction>[2]
        {
            new ShiftActionRelay(),
            new ShiftActionRelay()
        };
        [SerializeField] private PlayerActionRelay<BumperAction>[] secondRelays = new PlayerActionRelay<BumperAction>[2]
        {
            new NoteActionRelay(), 
            new NoteActionRelay()
        };

        void Start()
        {
            for (var i = 0; i < 2; i++)
            {
                firstRelays[i].SetSource(targets[i]);
                secondRelays[i].SetSource(targets[i]);
            }
            
            ReinitializeRelay();

            var roundHandler = Repository.Get<RoundHandler>();
            roundHandler[PhaseType.Acting].OnStart += ReinitializeRelay;
        }
        
        protected override bool CanBeExecuted() => Repository.Get<Player>().phase == 1;

        protected override void Execute(int beat, double offset)
        {
            if (beat == 0)
            {
                Debug.Log("Switching gameplay stance !");

                for (var i = 0; i < 2; i++) targets[i].SetRelay(secondRelays[i]);
                Repository.Get<Player>().phase = 2;
            }
            base.Execute(beat, offset);
        }

        private void ReinitializeRelay()
        {
            Repository.Get<Player>().phase = 1;
            for (var i = 0; i < 2; i++) targets[i].SetRelay(firstRelays[i]);
        }
    }
}