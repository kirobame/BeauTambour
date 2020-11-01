using System;

namespace BeauTambour
{
    public interface IPhase
    {
        event Action onEnd;
        
        PhaseType Type { get; }

        void Begin();
    }
}