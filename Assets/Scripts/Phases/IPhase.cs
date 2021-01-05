namespace BeauTambour
{
    public interface IPhase
    {
        PhaseCategory Category { get; }
        
        void Begin();
        void End();

        void Pause();
        void Resume();
    }
}