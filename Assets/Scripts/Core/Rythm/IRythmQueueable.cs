namespace BeauTambour
{
    public interface IRythmQueueable
    {
        double StartingTime { get; }
        int StartingBeat { get; }
        
        bool IsDone { get; }
        
        void Prepare(double startingTime, int startingBeat, double offset);

        void Tick(double time);
        void Beat(int count);
    }
}