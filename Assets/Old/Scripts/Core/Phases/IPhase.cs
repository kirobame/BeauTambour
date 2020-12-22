using System;

namespace Deprecated
{
    public interface IPhase
    {
        event Action onEnd;
        
        PhaseType Type { get; }

        void Begin();
        void End();
    }
}