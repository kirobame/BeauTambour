namespace Deprecated
{
    public interface IRythmQueueable
    {
        int Start { get; }
        bool IsDone { get; }
        double Offset { get; }
        
        void Prepare(int start, double offset);

        void Tick(double time);
        void Beat(int count);
    }
}