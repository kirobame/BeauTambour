namespace BeauTambour
{
    public abstract class RythmAction : IRythmQueueable
    {
        public int Start { get; private set; }
        public bool IsDone { get; protected set; }
        public double Offset { get; private set; }
        
        public virtual void Reset() => IsDone = false;
        
        public abstract void Tick(double time);
        public abstract void Beat(int count);

        void IRythmQueueable.Prepare(int start, double offset)
        {
            Start = start;
            Offset = offset;
        }
    }
}