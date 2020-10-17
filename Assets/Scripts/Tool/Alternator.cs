using UnityEngine;

namespace BeauTambour.Tooling
{
    public class Alternator : Executable
    {
        [SerializeField] private Executable first, second;

        private bool state;

        public override void Execute()
        {
            if (state == false)
            {
                second.Execute();
                state = true;
            }
            else
            {
                first.Execute();
                state = false;
            }
        }
    }
}